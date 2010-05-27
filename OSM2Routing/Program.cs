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

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, "C:\\temp\\test-orig.osm");

			OSMDB routable = target.BuildRoutableOSM();
			routable.Save("C:\\temp\\test-segments.osm");
			Console.WriteLine("Done.");
		}
	}
}
