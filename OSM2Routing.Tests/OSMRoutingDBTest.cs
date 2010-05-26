using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.OSMUtils.OSMDatabase;
using LK.OSM2Routing;
using System.IO;

namespace OSM2Routing.Tests {
	public class OSMRoutingDBTest {
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

			OSMRoutingDB target = new OSMRoutingDB();
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

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_ways_with_nodes));

			Assert.Equal(4, target.Nodes.Count);
			Assert.NotNull(target.Nodes[1]);
			Assert.NotNull(target.Nodes[2]);
			Assert.NotNull(target.Nodes[3]);
			Assert.NotNull(target.Nodes[4]);
		}

		[Fact()]
		public void OSMFiltredDBUsedNodesReturnsCorrectWaysAssociatedWithNode() {
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
			// </osm>

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_ways_with_nodes));

			Assert.NotNull(target.UsedNodes[3]);
			Assert.Contains(101, target.UsedNodes[3]);
			Assert.Contains(102, target.UsedNodes[3]);
		}


		[Fact()]
		public void OsmRoutingDBBuildRoutableOSMSplitsWaysAtCrossings() {
			RoadType acceptedRoad = new RoadType();
			acceptedRoad.RequiredTags.Add(new OSMTag("highway", "*"));

			//<way id="101">
			//  <nd ref="1" />
			//  <nd ref="2" />
			//  <nd ref="3" />
			//  <tag k="highway" v="residental" />
			//</way>

			//<way id="102">
			//  <nd ref="2" />
			//  <nd ref="4" />
			//  <tag k="highway" v="primary" />
			//</way>

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_simple_crossing));

			OSMDB routable = target.BuildRoutableOSM();

			Assert.Equal(3, routable.Ways.Count);
			Assert.True((routable.Ways[-1].Nodes[0] == 1 && routable.Ways[-1].Nodes[1] == 2) ||
									(routable.Ways[-2].Nodes[0] == 1 && routable.Ways[-2].Nodes[1] == 2) ||
									(routable.Ways[-3].Nodes[0] == 1 && routable.Ways[-3].Nodes[1] == 2));
			Assert.True((routable.Ways[-1].Nodes[0] == 2 && routable.Ways[-1].Nodes[1] == 3) ||
						      (routable.Ways[-2].Nodes[0] == 2 && routable.Ways[-2].Nodes[1] == 3) ||
						      (routable.Ways[-3].Nodes[0] == 2 && routable.Ways[-3].Nodes[1] == 3));
			Assert.True((routable.Ways[-1].Nodes[0] == 2 && routable.Ways[-1].Nodes[1] == 4) ||
						      (routable.Ways[-2].Nodes[0] == 2 && routable.Ways[-2].Nodes[1] == 4) ||
						      (routable.Ways[-3].Nodes[0] == 2 && routable.Ways[-3].Nodes[1] == 4));
		}
	}
}
