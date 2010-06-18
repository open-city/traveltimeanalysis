using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GeoUtils;
using LK.GeoUtils.Geometry;

namespace GeoUtils.Tests {
	public class SegmentTest {
		[Fact()]
		public void SegmentConstructorSetsStartAndEndPoint() {
			PointGeo startPoint = new PointGeo(21, 34);
			PointGeo endPoint = new PointGeo(26, 31);

			Segment<PointGeo> target = new Segment<PointGeo>(startPoint, endPoint);

			Assert.Equal(startPoint, target.StartPoint);
			Assert.Equal(endPoint, target.EndPoint);
		}

		[Fact()]
		public void SegmentLengthReturnsDistanceBetweenPoints() {
			PointGeo startPoint = new PointGeo(21, 34);
			PointGeo endPoint = new PointGeo(26, 31);

			Segment<PointGeo> target = new Segment<PointGeo>(startPoint, endPoint);

			double expectedLength = Calculations.GetDistance2D(startPoint, endPoint);
			Assert.InRange(target.Length, expectedLength - Calculations.EpsLength, expectedLength + Calculations.EpsLength);
		}
	}
}
