using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GPXUtils;

namespace GPXUtils.Tests {
	public class GPXSegmentTest {
		[Fact()]
		public void GPXSegmentTravelTimeReturnsTimeDifferenceBetweenStartAndEndPoint() {
			GPXPoint start = new GPXPoint(13, 15, new DateTime(2010, 5, 30, 12, 00, 00));
			GPXPoint end = new GPXPoint(13.1, 15.1, new DateTime(2010, 5, 30, 13, 05, 00));

			GPXSegment target = new GPXSegment(start, end);

			Assert.Equal(end.Time - start.Time, target.TravelTime);
		}

		[Fact()]
		public void GPXSegmentAverageSpeedReturnsAvarageSpeedOnTheSegment() {
			// Segment 1000 m length
			GPXPoint start = new GPXPoint(50.50673, 16.00795, new DateTime(2010, 5, 30, 12, 00, 00));
			GPXPoint end = new GPXPoint(50.51572, 16.00795, new DateTime(2010, 5, 30, 12, 02, 00));

			GPXSegment target = new GPXSegment(start, end);

			// 1km, 2 minutes => 30 km/h
			double expectedSpeed = 30;
			double error = Math.Abs(expectedSpeed - target.AverageSpeed);

			Assert.True(error < 0.1);
		}
	}
}
