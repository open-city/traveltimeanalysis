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
		public void OsmRoutingDBBuildRoutableOSMSplitsWaysAtTCrossing() {
			RoadType acceptedRoad = new RoadType();
			acceptedRoad.RequiredTags.Add(new OSMTag("highway", "*"));

			//  <way id="101">
			//    <nd ref="1" />
			//    <nd ref="2" />
			//    <nd ref="3" />
			//    <tag k="highway" v="residental" />
			//  </way>

			//  <way id="102">
			//    <nd ref="2" />
			//    <nd ref="4" />
			//    <tag k="highway" v="primary" />
			//  </way>

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_T_crossing));

			OSMDB routable = target.BuildRoutableOSM();

			Assert.Equal(3, routable.Ways.Count);
			Assert.Contains(new OSMWay(0, new int[] { 1, 2 }), routable.Ways, new WayNodesComparer());
			Assert.Contains(new OSMWay(0, new int[] { 2, 3 }), routable.Ways, new WayNodesComparer());
			Assert.Contains(new OSMWay(0, new int[] { 2, 4 }), routable.Ways, new WayNodesComparer());
		}

		[Fact()]
		public void OsmRoutingDBBuildRoutableOSMSplitsWaysAtXCrossing() {
			RoadType acceptedRoad = new RoadType();
			acceptedRoad.RequiredTags.Add(new OSMTag("highway", "*"));

			//  <way id="101">
			//    <nd ref="1" />
			//    <nd ref="2" />
			//    <nd ref="3" />
			//    <tag k="highway" v="residental" />
			//  </way>

			//  <way id="102">
			//    <nd ref="5" />
			//    <nd ref="2" />
			//    <nd ref="4" />
			//    <tag k="highway" v="primary" />
			//  </way>

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_X_crossing));

			OSMDB routable = target.BuildRoutableOSM();

			Assert.Equal(4, routable.Ways.Count);
			Assert.Contains(new OSMWay(0, new int[] { 1, 2 }), routable.Ways, new WayNodesComparer());
			Assert.Contains(new OSMWay(0, new int[] { 2, 3 }), routable.Ways, new WayNodesComparer());
			Assert.Contains(new OSMWay(0, new int[] { 5, 2 }), routable.Ways, new WayNodesComparer());
			Assert.Contains(new OSMWay(0, new int[] { 2, 4 }), routable.Ways, new WayNodesComparer());
		}

		[Fact()]
		public void OsmRoutingDBBuildRoutableOSMHandlesCircularWays() {
			RoadType acceptedRoad = new RoadType();
			acceptedRoad.RequiredTags.Add(new OSMTag("highway", "*"));

			//  <way id="101">
			//    <nd ref="1" />
			//    <nd ref="2" />
			//    <nd ref="3" />
			//    <nd ref="4" />
			//    <nd ref="1" />
			//    <tag k="highway" v="residental" />
			//  </way>

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_circular));

			OSMDB routable = target.BuildRoutableOSM();

			Assert.Equal(1, routable.Ways.Count);
			Assert.Contains(new OSMWay(0, new int[] { 1, 2, 3, 4, 1 }), routable.Ways, new WayNodesComparer());
		}

		[Fact()]
		public void OsmRoutingDBBuildRoutableOSMSplitCircularWays() {
			RoadType acceptedRoad = new RoadType();
			acceptedRoad.RequiredTags.Add(new OSMTag("highway", "*"));

			//  <way id="101">
			//    <nd ref="1" />
			//    <nd ref="2" />
			//    <nd ref="3" />
			//    <nd ref="4" />
			//    <nd ref="1" />
			//    <tag k="highway" v="residental" />
			//  </way>

			//  <way id="102">
			//    <nd ref="3" />
			//    <nd ref="5" />
			//    <tag k="highway" v="residental" />
			//  </way>

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_circular_with_other));

			OSMDB routable = target.BuildRoutableOSM();

			Assert.Equal(3, routable.Ways.Count);
			Assert.Contains(new OSMWay(0, new int[] { 1, 2, 3 }), routable.Ways, new WayNodesComparer());
			Assert.Contains(new OSMWay(0, new int[] { 3, 4, 1 }), routable.Ways, new WayNodesComparer());
			Assert.Contains(new OSMWay(0, new int[] { 3, 5 }), routable.Ways, new WayNodesComparer());
		}

		class WayNodesComparer : IEqualityComparer<OSMWay> {
			public bool Equals(OSMWay x, OSMWay y) {
				if (x.Nodes.Count == y.Nodes.Count) {
					for (int i = 0; i < x.Nodes.Count; i++) {
						if (x.Nodes[i] != y.Nodes[i]) {
							return false;
						}
					}

					return true;
				}

				return false;
			}

			public int GetHashCode(OSMWay obj) {
				throw new NotImplementedException();
			}
		}
	}
}
