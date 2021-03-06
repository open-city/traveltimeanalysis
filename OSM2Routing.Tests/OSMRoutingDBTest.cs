﻿//  Travel Time Analysis project
//  Copyright (C) 2010 Lukas Kabrt
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Xunit;

using LK.OSMUtils.OSMDatabase;
using LK.OSM2Routing;

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

		[Fact()]
		public void OsmRoutingDBBuildRoutableOSMAddsOriginalWayIDTag() {
			RoadType acceptedRoad = new RoadType();
			acceptedRoad.RequiredTags.Add(new OSMTag("highway", "*"));

			//  <way id="101">
			//    <nd ref="1" />
			//    <nd ref="2" />
			//    <nd ref="3" />
			//    <tag k="highway" v="residental" />
			//		<tag k="oneway" v="yes" />
			//  </way>

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_simple_way));

			OSMDB routable = target.BuildRoutableOSM();

			Assert.Equal(1, routable.Ways.Count);
			Assert.Equal("101", routable.Ways.Single().Tags["way-id"].Value);
		}

		[Fact()]
		public void OsmRoutingDBBuildRoutableOSMAddsAccessibleTags() {
			RoadType acceptedRoad = new RoadType();
			acceptedRoad.Speed = 50;
			acceptedRoad.RequiredTags.Add(new OSMTag("highway", "*"));

			//  <way id="101">
			//    <nd ref="1" />
			//    <nd ref="2" />
			//    <nd ref="3" />
			//    <tag k="highway" v="residental" />
			//		<tag k="oneway" v="yes" />
			//  </way>

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_simple_way));

			OSMDB routable = target.BuildRoutableOSM();

			Assert.Equal(1, routable.Ways.Count);
			Assert.Equal("yes", routable.Ways.Single().Tags["accessible"].Value);
			Assert.Equal("no", routable.Ways.Single().Tags["accessible-reverse"].Value);
		}

		[Fact()]
		public void OsmRoutingDBBuildRoutableOSMAddsSpeedTag() {
			RoadType acceptedRoad = new RoadType();
			acceptedRoad.Speed = 50;
			acceptedRoad.RequiredTags.Add(new OSMTag("highway", "*"));

			//  <way id="101">
			//    <nd ref="1" />
			//    <nd ref="2" />
			//    <nd ref="3" />
			//    <tag k="highway" v="residental" />
			//		<tag k="oneway" v="yes" />
			//  </way>

			OSMRoutingDB target = new OSMRoutingDB();
			target.Load(new RoadType[] { acceptedRoad }, new MemoryStream(TestData.osm_simple_way));

			OSMDB routable = target.BuildRoutableOSM();

			Assert.Equal(1, routable.Ways.Count);
			Assert.Equal("50", routable.Ways.Single().Tags["speed"].Value);
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
