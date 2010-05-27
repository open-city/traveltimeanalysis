using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NDesk.Options;
using LK.OSMUtils.OSMDatabase;

namespace LK.OSM2Routing {
	class Program {
		static void Main(string[] args) {
			RoadType accepted = new RoadType();
			accepted.RequiredTags.Add(new OSMTag("highway", "*"));

			OSMRoutingDB osm = new OSMRoutingDB();
			osm.Load(new RoadType[] {accepted},  "C:\\temp\\map.xml");

			osm.BuildRoutableOSM().Save("C:\\temp\\map-segment.xml");

			Console.WriteLine("Done.");
		}
	}
}
