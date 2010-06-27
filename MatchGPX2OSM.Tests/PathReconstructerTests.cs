using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Xunit;

using LK.MatchGPX2OSM;
using LK.GeoUtils;
using LK.GPXUtils;
using LK.OSMUtils.OSMDatabase;

namespace MatchGPX2OSM.Tests {
	public class PathReconstructerTests {
		[Fact()]
		public void ReconstructReturnsOneWayForPointsOnTheSameSegment() {
			OSMDB map = new OSMDB();
			map.Load(new MemoryStream(TestData.osm_reconstruct));

			RoadGraph graph = new RoadGraph();
			graph.Build(map);

			GPXDocument gps = new GPXDocument();
			// Track between nodes 30 and 31 in OSM map
			gps.Load(new MemoryStream(TestData.gpx_two_points_same_segment));

			STMatching matching = new STMatching(graph);
			var matched = matching.Match(gps.Tracks[0].Segments[0]);

			PathReconstructer target = new PathReconstructer(graph);
			var result = target.Reconstruct(matched, false);

			Assert.Equal(2, result.Nodes.Count);
			Assert.Equal(1, result.Ways.Count);
			Assert.Equal(2, result.Ways.First().Nodes.Count);

			var firstNode = result.Nodes[result.Ways.First().Nodes[0]];
			var secondNode = result.Nodes[result.Ways.First().Nodes[1]];

			Assert.True(Calculations.GetDistance2D(firstNode, map.Nodes[30]) < Calculations.EpsLength);
			Assert.True(Calculations.GetDistance2D(secondNode, map.Nodes[31]) < Calculations.EpsLength);
		}

		[Fact()]
		public void ReconstructSetsOriginalWayIDTag() {
			OSMDB map = new OSMDB();
			map.Load(new MemoryStream(TestData.osm_reconstruct));

			RoadGraph graph = new RoadGraph();
			graph.Build(map);

			GPXDocument gps = new GPXDocument();
			// Track between nodes 30 and 31 in OSM map
			gps.Load(new MemoryStream(TestData.gpx_two_points_same_segment));

			STMatching matching = new STMatching(graph);
			var matched = matching.Match(gps.Tracks[0].Segments[0]);

			PathReconstructer target = new PathReconstructer(graph);
			var result = target.Reconstruct(matched, false);

			Assert.True(result.Ways.First().Tags.ContainsTag("way-id"));
			Assert.Equal("1", result.Ways.First().Tags["way-id"].Value);
		}

		[Fact()]
		public void ReconstructSetsOriginalNodeIDTag() {
			OSMDB map = new OSMDB();
			map.Load(new MemoryStream(TestData.osm_reconstruct));

			RoadGraph graph = new RoadGraph();
			graph.Build(map);

			GPXDocument gps = new GPXDocument();
			// Track between nodes 30 and 31 in OSM map
			gps.Load(new MemoryStream(TestData.gpx_two_points_same_segment));

			STMatching matching = new STMatching(graph);
			var matched = matching.Match(gps.Tracks[0].Segments[0]);

			PathReconstructer target = new PathReconstructer(graph);
			var result = target.Reconstruct(matched, false);

			var nodeIds = result.Ways.First().Nodes;

			Assert.Equal(2, nodeIds.Count);
			Assert.True(result.Nodes[nodeIds[0]].Tags.ContainsTag("node-id"));
			Assert.Equal("30", result.Nodes[nodeIds[0]].Tags["node-id"].Value);
			Assert.True(result.Nodes[nodeIds[1]].Tags.ContainsTag("node-id"));
			Assert.Equal("31", result.Nodes[nodeIds[1]].Tags["node-id"].Value);
		}

		[Fact()]
		public void ReconstructSetsOriginalNodeIDTagOnMiddleNodes() {
			OSMDB map = new OSMDB();
			map.Load(new MemoryStream(TestData.osm_reconstruct));

			RoadGraph graph = new RoadGraph();
			graph.Build(map);

			GPXDocument gps = new GPXDocument();
			// Track between nodes 30 and 31 in OSM map
			gps.Load(new MemoryStream(TestData.gpx_two_points_same_way));

			STMatching matching = new STMatching(graph);
			var matched = matching.Match(gps.Tracks[0].Segments[0]);

			PathReconstructer target = new PathReconstructer(graph);
			var result = target.Reconstruct(matched, false);

			var nodeIds = result.Ways.First().Nodes;

			Assert.Equal(3, nodeIds.Count);
			Assert.True(result.Nodes[nodeIds[0]].Tags.ContainsTag("node-id"));
			Assert.Equal("30", result.Nodes[nodeIds[0]].Tags["node-id"].Value);
			Assert.True(result.Nodes[nodeIds[1]].Tags.ContainsTag("node-id"));
			Assert.Equal("31", result.Nodes[nodeIds[1]].Tags["node-id"].Value);
			Assert.True(result.Nodes[nodeIds[2]].Tags.ContainsTag("node-id"));
			Assert.Equal("32", result.Nodes[nodeIds[2]].Tags["node-id"].Value);
		}

		[Fact()]
		public void ReconstructReturnsOneWayForPointsOnTheSameSegmentReverse() {
			OSMDB map = new OSMDB();
			map.Load(new MemoryStream(TestData.osm_reconstruct));

			RoadGraph graph = new RoadGraph();
			graph.Build(map);

			GPXDocument gps = new GPXDocument();
			// Track between nodes 31 and 30 in OSM map
			gps.Load(new MemoryStream(TestData.gpx_two_points_same_segment_reverse));

			STMatching matching = new STMatching(graph);
			var matched = matching.Match(gps.Tracks[0].Segments[0]);

			PathReconstructer target = new PathReconstructer(graph);
			var result = target.Reconstruct(matched, false);

			Assert.Equal(2, result.Nodes.Count);
			Assert.Equal(1, result.Ways.Count);
			Assert.Equal(2, result.Ways.First().Nodes.Count);

			var firstNode = result.Nodes[result.Ways.First().Nodes[0]];
			var secondNode = result.Nodes[result.Ways.First().Nodes[1]];

			Assert.True(Calculations.GetDistance2D(firstNode, map.Nodes[31]) < Calculations.EpsLength);
			Assert.True(Calculations.GetDistance2D(secondNode, map.Nodes[30]) < Calculations.EpsLength);
		}

		[Fact()]
		public void ReconstructReturnsOneWayForPointsOnTheSameWay() {
			OSMDB map = new OSMDB();
			map.Load(new MemoryStream(TestData.osm_reconstruct));

			RoadGraph graph = new RoadGraph();
			graph.Build(map);

			GPXDocument gps = new GPXDocument();
			// Track between nodes 30 and 32 in OSM map, node 31 is between them
			gps.Load(new MemoryStream(TestData.gpx_two_points_same_way));

			STMatching matching = new STMatching(graph);
			var matched = matching.Match(gps.Tracks[0].Segments[0]);

			PathReconstructer target = new PathReconstructer(graph);
			var result = target.Reconstruct(matched, false);

			Assert.Equal(3, result.Nodes.Count);
			Assert.Equal(1, result.Ways.Count);
			Assert.Equal(3, result.Ways.First().Nodes.Count);

			var firstNode = result.Nodes[result.Ways.First().Nodes[0]];
			var middleNode = result.Nodes[result.Ways.First().Nodes[1]];
			var lastNode = result.Nodes[result.Ways.First().Nodes[2]];

			Assert.True(Calculations.GetDistance2D(firstNode, map.Nodes[30]) < Calculations.EpsLength);
			Assert.True(Calculations.GetDistance2D(middleNode, map.Nodes[31]) < Calculations.EpsLength);
			Assert.True(Calculations.GetDistance2D(lastNode, map.Nodes[32]) < Calculations.EpsLength);
		}

		[Fact()]
		public void ReconstructReturnsOneWayForPointsOnTheSameWayReverse() {
			OSMDB map = new OSMDB();
			map.Load(new MemoryStream(TestData.osm_reconstruct));

			RoadGraph graph = new RoadGraph();
			graph.Build(map);

			GPXDocument gps = new GPXDocument();
			// Track between nodes 32 and 30 in OSM map, node 31 is between them
			gps.Load(new MemoryStream(TestData.gpx_two_points_same_way_reverse));

			STMatching matching = new STMatching(graph);
			var matched = matching.Match(gps.Tracks[0].Segments[0]);

			PathReconstructer target = new PathReconstructer(graph);
			var result = target.Reconstruct(matched, false);

			Assert.Equal(3, result.Nodes.Count);
			Assert.Equal(1, result.Ways.Count);
			Assert.Equal(3, result.Ways.First().Nodes.Count);

			var firstNode = result.Nodes[result.Ways.First().Nodes[0]];
			var middleNode = result.Nodes[result.Ways.First().Nodes[1]];
			var lastNode = result.Nodes[result.Ways.First().Nodes[2]];

			Assert.True(Calculations.GetDistance2D(firstNode, map.Nodes[32]) < Calculations.EpsLength);
			Assert.True(Calculations.GetDistance2D(middleNode, map.Nodes[31]) < Calculations.EpsLength);
			Assert.True(Calculations.GetDistance2D(lastNode, map.Nodes[30]) < Calculations.EpsLength);
		}

		[Fact()]
		public void ReconstructReturnsTwoWayForPointsOnDifferentWays() {
			OSMDB map = new OSMDB();
			map.Load(new MemoryStream(TestData.osm_reconstruct));

			RoadGraph graph = new RoadGraph();
			graph.Build(map);

			GPXDocument gps = new GPXDocument();
			// Track between nodes 22 and 32 in OSM map, nodes 21, 10, 33 are between them
			gps.Load(new MemoryStream(TestData.gpx_two_points_different_ways));

			STMatching matching = new STMatching(graph);
			var matched = matching.Match(gps.Tracks[0].Segments[0]);

			PathReconstructer target = new PathReconstructer(graph);
			var result = target.Reconstruct(matched, false);

			Assert.Equal(5, result.Nodes.Count);
			Assert.Equal(2, result.Ways.Count);
			Assert.Equal(3, result.Ways.First().Nodes.Count);
			Assert.Equal(3, result.Ways.ToList()[1].Nodes.Count);


			var firstNode1 = result.Nodes[result.Ways.First().Nodes[0]];
			var middleNode1 = result.Nodes[result.Ways.First().Nodes[1]];
			var lastNode1 = result.Nodes[result.Ways.First().Nodes[2]];

			var firstNode2 = result.Nodes[result.Ways.ToList()[1].Nodes[0]];
			var middleNode2 = result.Nodes[result.Ways.ToList()[1].Nodes[1]];
			var lastNode2 = result.Nodes[result.Ways.ToList()[1].Nodes[2]];

			Assert.True(Calculations.GetDistance2D(firstNode1, map.Nodes[22]) < Calculations.EpsLength);
			Assert.True(Calculations.GetDistance2D(middleNode1, map.Nodes[21]) < Calculations.EpsLength);
			Assert.True(Calculations.GetDistance2D(lastNode1, map.Nodes[10]) < Calculations.EpsLength);

			Assert.True(Calculations.GetDistance2D(firstNode2, map.Nodes[10]) < Calculations.EpsLength);
			Assert.True(Calculations.GetDistance2D(middleNode2, map.Nodes[33]) < Calculations.EpsLength);
			Assert.True(Calculations.GetDistance2D(lastNode2, map.Nodes[32]) < Calculations.EpsLength);
		}

		[Fact()]
		public void FilterPrecessesSimpleUTurnCorrectly() {
			OSMDB original = new OSMDB();
			original.Load(new MemoryStream(TestData.osm_simle_uturn));

			OSMDB target = new OSMDB();
			target.Load(new MemoryStream(TestData.osm_simle_uturn));
			PathReconstructer.FilterUTurns(target, 100);

			Assert.Equal(3, target.Nodes.Count);
			Assert.Equal(2, target.Ways.Count);

			Assert.Equal(1, target.Ways.OrderBy(w => w.Tags["order"].Value).First().Nodes[0]);
			Assert.Equal(6, target.Ways.OrderBy(w => w.Tags["order"].Value).First().Nodes[1]);

			Assert.Equal(6, target.Ways.OrderBy(w => w.Tags["order"].Value).Last().Nodes[0]);
			Assert.Equal(7, target.Ways.OrderBy(w => w.Tags["order"].Value).Last().Nodes[1]);
		}

		[Fact()]
		public void FilterPrecessesSimpleUTurnOnOneWayCorrectly() {
			OSMDB original = new OSMDB();
			original.Load(new MemoryStream(TestData.osm_uturns_one_way));

			OSMDB target = new OSMDB();
			target.Load(new MemoryStream(TestData.osm_uturns_one_way));
			PathReconstructer.FilterUTurns(target, 100);

			Assert.Equal(4, target.Nodes.Count);
			Assert.Equal(3, target.Ways.Count);

			var ways = target.Ways.OrderBy(w => int.Parse(w.Tags["order"].Value)).ToList();

			Assert.Equal(1, ways[0].Nodes[0]);
			Assert.Equal(4, ways[0].Nodes[1]);

			Assert.Equal(4, ways[1].Nodes[0]);
			Assert.Equal(5, ways[1].Nodes[1]);

			Assert.Equal(5, ways[2].Nodes[0]);
			Assert.Equal(6, ways[2].Nodes[1]);
		}

		[Fact()]
		public void FilterPrecessesMoreUTurnsOnOneWayCorrectly() {
			OSMDB original = new OSMDB();
			original.Load(new MemoryStream(TestData.osm_more_uturns_one_way));

			OSMDB target = new OSMDB();
			target.Load(new MemoryStream(TestData.osm_more_uturns_one_way));
			PathReconstructer.FilterUTurns(target, 100);

			Assert.Equal(9, target.Nodes.Count);
			Assert.Equal(3, target.Ways.Count);

			var ways = target.Ways.OrderBy(w => int.Parse(w.Tags["order"].Value)).ToList();

			Assert.Equal(1, ways[0].Nodes[0]);
			Assert.Equal(25, ways[0].Nodes[1]);

			Assert.Equal(25, ways[1].Nodes[0]);
			Assert.Equal(26, ways[1].Nodes[1]);
			Assert.Equal(27, ways[1].Nodes[2]);
			Assert.Equal(28, ways[1].Nodes[3]);
			Assert.Equal(29, ways[1].Nodes[4]);
			Assert.Equal(30, ways[1].Nodes[5]);
			Assert.Equal(31, ways[1].Nodes[6]);

			Assert.Equal(31, ways[2].Nodes[0]);
			Assert.Equal(32, ways[2].Nodes[1]);
		}

		[Fact()]
		public void FilterPrecessesTestCase1Correctly() {
			OSMDB original = new OSMDB();
			original.Load(new MemoryStream(TestData.osm_test_case_1));

			OSMDB target = new OSMDB();
			target.Load(new MemoryStream(TestData.osm_test_case_1));
			PathReconstructer.FilterUTurns(target, 100);

			Assert.Equal(4, target.Nodes.Count);
			Assert.Equal(3, target.Ways.Count);

			var ways = target.Ways.OrderBy(w => int.Parse(w.Tags["order"].Value)).ToList();

			Assert.Equal(1, ways[0].Nodes[0]);
			Assert.Equal(2, ways[0].Nodes[1]);

			Assert.Equal(2, ways[1].Nodes[0]);
			Assert.Equal(7, ways[1].Nodes[1]);

			Assert.Equal(7, ways[2].Nodes[0]);
			Assert.Equal(8, ways[2].Nodes[1]);
		}

		[Fact()]
		public void FilterPrecessesTestCase2Correctly() {
			OSMDB original = new OSMDB();
			original.Load(new MemoryStream(TestData.osm_test_case_2));

			OSMDB target = new OSMDB();
			target.Load(new MemoryStream(TestData.osm_test_case_2));
			PathReconstructer.FilterUTurns(target, 100);

			Assert.Equal(6, target.Nodes.Count);
			Assert.Equal(4, target.Ways.Count);

			var ways = target.Ways.OrderBy(w => int.Parse(w.Tags["order"].Value)).ToList();

			Assert.Equal(1, ways[0].Nodes[0]);
			Assert.Equal(2, ways[0].Nodes[1]);

			Assert.Equal(2, ways[1].Nodes[0]);
			Assert.Equal(3, ways[1].Nodes[1]);
			Assert.Equal(4, ways[1].Nodes[2]);

			Assert.Equal(4, ways[2].Nodes[0]);
			Assert.Equal(13, ways[2].Nodes[1]);

			Assert.Equal(13, ways[3].Nodes[0]);
			Assert.Equal(15, ways[3].Nodes[1]);
		}
	}
}
