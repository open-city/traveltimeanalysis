using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Xunit;

using LK.MatchGPX2OSM;
using LK.GeoUtils.Geometry;
using LK.OSMUtils.OSMDatabase;
using LK.GPXUtils;

namespace MatchGPX2OSM.Tests {
	public class STMatchingTest {
		[Fact()]
		public void STMatchingFindCandidatesPointsReturnsPointsOnCorrectSegments() {
			OSMDB map = new OSMDB();
			map.Load(new MemoryStream(TestData.osm_find_candidates_test));

			RoadGraph graph = new RoadGraph();
			graph.Build(map);

			STMatching target = new STMatching(graph);

			GPXPoint pt = new GPXPoint(50.49817, 16.10971);
			var candidates = target.FindCandidatePoints(pt);

			Assert.Equal(2, candidates.Count());
			
			var wayids = candidates.Select(c => c.Road.WayID);
			Assert.Contains(2, wayids);
			Assert.Contains(3, wayids);
		}
	}
}
