using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NDesk.Options;

namespace MatchGPX2OSM {
	class Program {
		static void Main(string[] args) {
			string osmPath = "";
			string gpxPath = "";
			string outputPath = "";
			bool showHelp = false;

			OptionSet parameters = new OptionSet() {
				{ "osm=",					v => osmPath = v},
				{ "gpx=",					v => gpxPath = v},
				{ "o|output=",			v => outputPath = v},
				{ "h|?|help",				v => showHelp = v != null},
			};

			try {
				parameters.Parse(args);
			}
			catch (OptionException e) {
				Console.Write("MatchGPX2OSM: ");
				Console.WriteLine(e.Message);
				Console.WriteLine("Try `matchgpx2osm --help' for more information.");
				return;
			}


			if (showHelp) {
				ShowHelp(parameters);
				return;
			}


		}

		/// <summary>
		/// Prints a help message
		/// </summary>
		/// <param name="p">The parameters accepted by this program</param>
		static void ShowHelp(OptionSet p) {
			Console.WriteLine("Usage: matchgpx2osm [OPTIONS]+");
			Console.WriteLine("Matches GPX track to the OSM map");
			Console.WriteLine();

			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Out);
		}
	}
}
