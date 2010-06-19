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

		[Fact()]
		public void SegmentEqualsReturnsTrueForSegmentsWithTheSameEndPoints() {
			PointGeo startPoint = new PointGeo(21, 34);
			PointGeo endPoint = new PointGeo(26, 31);

			Segment<PointGeo> target = new Segment<PointGeo>(startPoint, endPoint);
			Segment<PointGeo> theSame = new Segment<PointGeo>(startPoint, endPoint);

			Assert.True(target.Equals(theSame));
		}

		[Fact()]
		public void SegmentEqualsReturnsFalseForNull() {
			PointGeo startPoint = new PointGeo(21, 34);
			PointGeo endPoint = new PointGeo(26, 31);

			Segment<PointGeo> target = new Segment<PointGeo>(startPoint, endPoint);

			Assert.False(target.Equals(null));
		}

		[Fact()]
		public void SegmentEqualsReturnsFalseForSegmentsWithTheDifferentEndPoints() {
			PointGeo startPoint = new PointGeo(21, 34);
			PointGeo startPoint2 = new PointGeo(21.1, 34.1);

			PointGeo endPoint = new PointGeo(26, 31);

			Segment<PointGeo> target = new Segment<PointGeo>(startPoint, endPoint);
			Segment<PointGeo> other = new Segment<PointGeo>(startPoint2, endPoint);

			Assert.False(target.Equals(other));
		}
	}
}
