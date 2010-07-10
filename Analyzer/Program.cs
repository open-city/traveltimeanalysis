using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NDesk.Options;

using LK.OSMUtils.OSMDatabase;

namespace LK.Analyzer {
	class Program {
		static void Main(string[] args) {
			string dbPath = "";
			string mapPath = "";
			string trackPath = "";
			string outputPath = ".";
			bool addTracks = false;
			bool showHelp = false;
			bool analyze = false;

			OptionSet parameters = new OptionSet() {
				{ "db=", "path to the travel times database",																									v => dbPath = v},
				{ "add", "adds specified tracks to the DB",																										v => addTracks = v != null},
				{ "track=",	"path to the matched GPS track to process or to the directory to process",				v => trackPath = v},
				{ "map=", "path to the routable map",																													v => mapPath = v},
				{ "a|analyze",																																								v => analyze = v != null},
				{ "o|output=", "path to the output directory",																								v => outputPath = v},
				{ "h|?|help",																																									v => showHelp = v != null},
			};

			try {
				parameters.Parse(args);
			}
			catch (OptionException e) {
				Console.Write("Analyzer: ");
				Console.WriteLine(e.Message);
				Console.WriteLine("Try `analyzer --help' for more information.");
				return;
			}

			if (showHelp || string.IsNullOrEmpty(dbPath) || string.IsNullOrEmpty(outputPath)) {
				ShowHelp(parameters);
				return;
			}

			Console.Write("Loading travel times database ...");
			XmlTravelTimeDB db = new XmlTravelTimeDB();
			if (File.Exists(dbPath)) {
				db.Load(dbPath);
			}
			Console.WriteLine("\t\t\tDone.");

			if (addTracks) {
				if (File.Exists(trackPath)) {
					AddTrackToDB(db, trackPath);
				}
				else if (Directory.Exists(trackPath)) {
					var files = Directory.GetFiles(trackPath, "*.osm");
					Console.WriteLine("Found {0} GPX file(s).", files.Length);

					foreach (var file in files) {
						AddTrackToDB(db, file);
					}
				}

				Console.Write("Saving travel times database ...");
				db.Save(dbPath);
				Console.WriteLine("\t\t\tDone.");
			}

			if (analyze) {
				Console.Write("Loading routable map ...");
				OSMDB map = new OSMDB();
				map.Load(mapPath);

				Console.WriteLine("\t\t\tDone.");

				TTAnalyzer analyzer = new TTAnalyzer(map);
				foreach (var segment in db.TravelTimesSegments) {
					Model result = analyzer.Analyze(db.GetTravelTimes(segment), segment);
				}
			}
		}

		static void AddTrackToDB(ITravelTimesDB db, string path) {
			OSMDB track = new OSMDB();

			Console.Write("Loading {0} ...", Path.GetFileName(path));
			track.Load(path);

			try {
				var travelTimes = TravelTime.FromMatchedTrack(track);
				foreach (var travelTime in travelTimes) {
					db.AddTravelTime(travelTime);
				}

				Console.WriteLine(".");
			}
			catch (Exception e) {
				Console.WriteLine("Error: " + e.Message);
			}
		}
		
		/// <summary>
		/// Prints a help message
		/// </summary>
		/// <param name="p">The parameters accepted by this program</param>
		static void ShowHelp(OptionSet p) {
			Console.WriteLine("Usage: analyzer [OPTIONS]+");
			Console.WriteLine("Analyzes matched GPS tracks.");
			Console.WriteLine();

			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Out);
		}
	}
}
