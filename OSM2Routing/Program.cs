using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NDesk.Options;
using LK.OSMUtils.OSMDatabase;

namespace LK.OSM2Routing {
	class Program {
		static void Main(string[] args) {
			RoadType acceptedRoad = new RoadType();
			acceptedRoad.RequiredTags.Add(new OSMTag("highway", "*"));

			DateTime start = DateTime.Now;

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, "C:\\temp\\czech_republic.osm");

			OSMDB routable = target.BuildRoutableOSM();
			routable.Save("C:\\temp\\test-segments.osm");

			Console.WriteLine(DateTime.Now - start);
			Console.WriteLine("Done.");
		}
	}
}
