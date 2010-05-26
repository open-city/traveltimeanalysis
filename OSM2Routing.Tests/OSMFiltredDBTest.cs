using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.OSMUtils.OSMDatabase;
using LK.OSM2Routing;
using System.IO;

namespace OSM2Routing.Tests {
	public class OSMFiltredDBTest {
		[Fact()]
		public void OSMFiltredDBLoadFiltersWaysThatDontMatchRoadTypes() {
			RoadType acceptedRoad = new RoadType();
			acceptedRoad.RequiredTags.Add(new OSMTag("highway", "*"));

			// <osm>
			//  <way id="101">
			//    <nd ref="1" />
			//    <nd ref="2" />
			//    <tag k="highway" v="residental" />
			//  </way>

			//  <way id="102">
			//    <nd ref="3" />
			//    <nd ref="4" />
			//    <tag k="highway" v="primary" />
			//  </way>

			//  <way id="103">
			//    <nd ref="5" />
			//    <nd ref="6" />
			//    <tag k="builing" v="yes" />
			//  </way>

			//  <way id="104">
			//    <nd ref="7" />
			//    <nd ref="8" />
			//    <tag k="highway" v="track" />
			//    <tag k="grade" v="1"/>
			//  </way>
			//</osm>

			OSMFilteredDB target = new OSMFilteredDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_ways_various_tags));

			Assert.Equal(3, target.Ways.Count);
			Assert.NotNull(target.Ways[101]);
			Assert.NotNull(target.Ways[102]);
			Assert.NotNull(target.Ways[104]);
		}

		[Fact()]
		public void OSMFiltredDBLoadLoadsNodesForMatchedWays() {
			RoadType acceptedRoad = new RoadType();
			acceptedRoad.RequiredTags.Add(new OSMTag("highway", "*"));

			// <osm>
			//  <way id="101">
			//    <nd ref="1" />
			//    <nd ref="2" />
			//    <nd ref="3" />
			//    <tag k="highway" v="residental" />
			//  </way>

			//  <way id="102">
			//    <nd ref="3" />
			//    <nd ref="4" />
			//    <tag k="highway" v="primary" />
			//  </way>

			//  <way id="103">
			//    <nd ref="5" />
			//    <nd ref="6" />
			//    <tag k="builing" v="yes" />
			//  </way>
			//</osm>

			OSMFilteredDB target = new OSMFilteredDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_ways_with_nodes));

			Assert.Equal(4, target.Nodes.Count);
			Assert.NotNull(target.Nodes[1]);
			Assert.NotNull(target.Nodes[2]);
			Assert.NotNull(target.Nodes[3]);
			Assert.NotNull(target.Nodes[4]);
		}
	}
}
