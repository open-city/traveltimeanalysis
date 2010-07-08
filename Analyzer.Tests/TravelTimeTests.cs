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

		[Fact()]
		public void FromMatchedTrackProcessesTrackThatStartsAndEndsWithCompleteSegments() {
			OSMDB track = new OSMDB();
			track.Load(new MemoryStream(TestData.osm_2_segments_without_incolmpete_parts));

			var target = TravelTime.FromMatchedTrack(track).ToList();

			Assert.Equal(2, target.Count());

			SegmentInfo expectedFirstSegment = new SegmentInfo() { NodeFromID = 411888806, NodeToID = 415814332, WayID = 36881783 };
			SegmentInfo expectedSecondSegment = new SegmentInfo() { NodeFromID = 415814332, NodeToID = 74165639, WayID = 36881783 };

			Assert.Equal(expectedFirstSegment, target[0].Segment);
			Assert.Equal(expectedSecondSegment, target[1].Segment);
		}

		[Fact()]
		public void FromMatchedTrackProcessesTrackWithManySegmentsSegments() {
			OSMDB track = new OSMDB();
			track.Load(new MemoryStream(TestData.osm_5_segments));

			var target = TravelTime.FromMatchedTrack(track).ToList();

			Assert.Equal(5, target.Count());

			SegmentInfo expectedFirstSegment = new SegmentInfo() { NodeFromID = 411888806, NodeToID = 415814332, WayID = 36881783 };
			SegmentInfo expectedSecondSegment = new SegmentInfo() { NodeFromID = 415814332, NodeToID = 74165639, WayID = 36881783 };
			SegmentInfo expectedThirdSegment = new SegmentInfo() { NodeFromID = 74165639, NodeToID = 320275336, WayID = 29115419 };
			SegmentInfo expectedFourthSegment = new SegmentInfo() { NodeFromID = 320275336, NodeToID = 415814335, WayID = 29115419 };
			SegmentInfo expectedFifthSegment = new SegmentInfo() { NodeFromID = 415814335, NodeToID = 415814327, WayID = 29115419 };

			Assert.Equal(expectedFirstSegment, target[0].Segment);
			Assert.Equal(expectedSecondSegment, target[1].Segment);
			Assert.Equal(expectedThirdSegment, target[2].Segment);
			Assert.Equal(expectedFourthSegment, target[3].Segment);
			Assert.Equal(expectedFifthSegment, target[4].Segment);
		}
	}
}
