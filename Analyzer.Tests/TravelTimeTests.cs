using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Xunit;

using LK.Analyzer;
using LK.OSMUtils.OSMDatabase;

namespace Analyzer.Tests {
	public class TravelTimeTests {
		[Fact()]
		public void FromMatchedTrackIgnoresFirstAndLastIncompleteSegments() {
			OSMDB track = new OSMDB();
			track.Load(new MemoryStream(TestData.osm_2_complete_segments));

			var target = TravelTime.FromMatchedTrack(track).ToList();

			Assert.Equal(2, target.Count());

			SegmentInfo expectedFirstSegment = new SegmentInfo() { NodeFromID = 411888806, NodeToID = 415814332, WayID = 36881783 };
			SegmentInfo expectedSecondSegment = new SegmentInfo() { NodeFromID = 415814332, NodeToID = 74165639, WayID = 36881783 };

			Assert.Equal(expectedFirstSegment, target[0].Segment);
			Assert.Equal(expectedSecondSegment, target[1].Segment);
		}
	}
}
