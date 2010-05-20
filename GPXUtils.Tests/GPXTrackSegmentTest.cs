using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GPXUtils;

namespace GPXUtils.Tests {
	public class GPXTrackSegmentTest {
		[Fact()]
		public void GPXTrackSegmentConstructorCreatesEmptySegment() {
			GPXTrackSegment target = new GPXTrackSegment();

			Assert.Equal(0, target.NodesCount);
		}

		[Fact()]
		public void GPXTrackSegmentConstructorAcceptsListOfGPXPoints() {
			GPXPoint pt1 = new GPXPoint(10.4, 5.2);
			GPXPoint pt2 = new GPXPoint(10.8, 5.3);

			GPXTrackSegment target = new GPXTrackSegment(new GPXPoint[] { pt1, pt2 });

			Assert.Equal(2, target.NodesCount);
			Assert.Equal(pt1, target.Nodes[0]);
			Assert.Equal(pt2, target.Nodes[1]);
		}
	}
}
