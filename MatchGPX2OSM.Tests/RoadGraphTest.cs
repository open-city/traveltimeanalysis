using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.MatchGPX2OSM;
using LK.OSMUtils.OSMDatabase;

namespace MatchGPX2OSM.Tests {
	public class RoadGraphTest {
		[Fact()]
		public void RoadGraphConstructorInitializesProperties() {
			RoadGraph target = new RoadGraph();

			Assert.NotNull(target.Nodes);
			Assert.NotNull(target.Connections);
		}

		[Fact()]
		public void RoadGraphBuildCreatesTwoWayConnectionbetweenPoints() {
			OSMDB map = new OSMDB();
			map.Nodes.Add(new OSMNode(1, 1, 1));
			map.Nodes.Add(new OSMNode(2, 2, 2));

			OSMWay way = new OSMWay(3, new List<int>(new int[] { 1,2 }));
			way.Tags.Add(new OSMTag("accessible", "yes"));
			way.Tags.Add(new OSMTag("accessible-reverse", "yes"));
			way.Tags.Add(new OSMTag("speed", "50"));
			way.Tags.Add(new OSMTag("way-id", "123"));
			map.Ways.Add(way);

			RoadGraph target = new RoadGraph();
			target.Build(map);

			Assert.Equal(2, target.Connections.Count());
			Assert.Equal(1, target.Connections.Where(c => c.From.MapPoint == map.Nodes[1] && c.To.MapPoint == map.Nodes[2]).Count());
			Assert.Equal(1, target.Connections.Where(c => c.From.MapPoint == map.Nodes[2] && c.To.MapPoint == map.Nodes[1]).Count());

			Assert.Equal(2, target.Nodes.Where(n => n.MapPoint == map.Nodes[1]).Single().Connections.Count());
			Assert.Equal(2, target.Nodes.Where(n => n.MapPoint == map.Nodes[2]).Single().Connections.Count());
		}

		[Fact()]
		public void RoadGraphBuildCreatesOneWayConnectionForOnewayRoads1() {
			OSMDB map = new OSMDB();
			map.Nodes.Add(new OSMNode(1, 1, 1));
			map.Nodes.Add(new OSMNode(2, 2, 2));

			OSMWay way = new OSMWay(3, new List<int>(new int[] { 1, 2 }));
			way.Tags.Add(new OSMTag("accessible", "yes"));
			way.Tags.Add(new OSMTag("accessible-reverse", "no"));
			way.Tags.Add(new OSMTag("speed", "50"));
			way.Tags.Add(new OSMTag("way-id", "123"));
			map.Ways.Add(way);

			RoadGraph target = new RoadGraph();
			target.Build(map);

			Assert.Equal(1, target.Connections.Count());
			Assert.Equal(1, target.Connections.Where(c => c.From.MapPoint == map.Nodes[1] && c.To.MapPoint == map.Nodes[2]).Count());

			Assert.Equal(1, target.Nodes.Where(n => n.MapPoint == map.Nodes[1]).Single().Connections.Count());
			Assert.Equal(1, target.Nodes.Where(n => n.MapPoint == map.Nodes[2]).Single().Connections.Count());
		}

		[Fact()]
		public void RoadGraphBuildCreatesOneWayConnectionForOnewayRoads2() {
			OSMDB map = new OSMDB();
			map.Nodes.Add(new OSMNode(1, 1, 1));
			map.Nodes.Add(new OSMNode(2, 2, 2));

			OSMWay way = new OSMWay(3, new List<int>(new int[] { 1, 2 }));
			way.Tags.Add(new OSMTag("accessible", "no"));
			way.Tags.Add(new OSMTag("accessible-reverse", "yes"));
			way.Tags.Add(new OSMTag("speed", "50"));
			way.Tags.Add(new OSMTag("way-id", "123"));
			map.Ways.Add(way);

			RoadGraph target = new RoadGraph();
			target.Build(map);

			Assert.Equal(1, target.Connections.Count());
			Assert.Equal(1, target.Connections.Where(c => c.From.MapPoint == map.Nodes[2] && c.To.MapPoint == map.Nodes[1]).Count());

			Assert.Equal(1, target.Nodes.Where(n => n.MapPoint == map.Nodes[1]).Single().Connections.Count());
			Assert.Equal(1, target.Nodes.Where(n => n.MapPoint == map.Nodes[2]).Single().Connections.Count());
		}

		[Fact()]
		public void RoadGraphBuildCreatesNodesAccordingToMap() {
			OSMDB map = new OSMDB();
			map.Nodes.Add(new OSMNode(1, 1, 2));
			map.Nodes.Add(new OSMNode(2, 2, 3));

			OSMWay way = new OSMWay(3, new List<int>(new int[] { 1, 2 }));
			way.Tags.Add(new OSMTag("accessible", "no"));
			way.Tags.Add(new OSMTag("accessible-reverse", "yes"));
			way.Tags.Add(new OSMTag("speed", "50"));
			way.Tags.Add(new OSMTag("way-id", "123"));
			map.Ways.Add(way);

			RoadGraph target = new RoadGraph();
			target.Build(map);

			Assert.Equal(2, target.Nodes.Count());
			Assert.Equal(1, target.Nodes.Where(n => n.MapPoint == map.Nodes[1]).Count());
			Assert.Equal(1, target.Nodes.Where(n => n.MapPoint == map.Nodes[2]).Count());
		}

		[Fact()]
		public void RoadGraphBuildCreatesCorrectConnectionGeometry() {
			OSMDB map = new OSMDB();
			map.Nodes.Add(new OSMNode(1, 1, 2));
			map.Nodes.Add(new OSMNode(2, 2, 3));
			map.Nodes.Add(new OSMNode(3, 3, 4));

			OSMWay way = new OSMWay(4, new int[] { 1, 2, 3 });
			way.Tags.Add(new OSMTag("accessible", "yes"));
			way.Tags.Add(new OSMTag("accessible-reverse", "yes"));
			way.Tags.Add(new OSMTag("speed", "50"));
			way.Tags.Add(new OSMTag("way-id", "123"));
			map.Ways.Add(way);

			RoadGraph target = new RoadGraph();
			target.Build(map);

			Assert.Equal(1, target.ConnectionGeometries.Count());
			Assert.Equal(3, target.ConnectionGeometries.Single().Nodes.Count);
		}
	}
}
