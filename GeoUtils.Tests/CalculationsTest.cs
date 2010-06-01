using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GeoUtils;
using LK.GeoUtils.Geometry;

namespace GeoUtils.Tests {
	public class CalculationsTest {
		[Fact()]
		public void CalculationsGetDistanceComputesDistanceBetweenPointsUsingGCD() {
			double error = Math.Abs(Calculations.GetDistance2D(new PointGeo(36.12, -86.67), new PointGeo(33.94, -118.4)) - 2886449);
			Assert.True(error < 1);
		}

		[Fact()]
		public void CalculationsGetLengthReturns0ForEmptyWay() {
			Polyline<IPointGeo> line = new Polyline<IPointGeo>();
			double length = Calculations.GetLength(line);

			Assert.Equal(0, length);
		}

		[Fact()]
		public void CalculationsGetLengthReturnsLengthAsSumOfDistanceBetweenPoints() {
			Polyline<IPointGeo> line = new Polyline<IPointGeo>();
			PointGeo pt1 = new PointGeo(50, 16);
			PointGeo pt2 = new PointGeo(50.1, 16.1);
			PointGeo pt3 = new PointGeo(50.2, 16.3);
			line.Nodes.Add(pt1);
			line.Nodes.Add(pt2);
			line.Nodes.Add(pt3);

			double exceptedLength = Calculations.GetDistance2D(pt1, pt2) + Calculations.GetDistance2D(pt2, pt3);
			double error = Math.Abs(exceptedLength - Calculations.GetLength(line));

			Assert.True(error < 0.1);
		}

		[Fact()]
		public void CalculationsGetDistanceReturnsDistanceFromLineSegmentForPointProjetedToSegment() {
			PointGeo orgin = new PointGeo(1, 1);
			PointGeo end = new PointGeo(2, 2);
			Segment<IPointGeo> segment = new Segment<IPointGeo>(orgin, end);

			PointGeo testPoint = new PointGeo(1, 2);

			Assert.Equal(Calculations.GetDistance2D(testPoint, new PointGeo(1.5,1.5)), Calculations.GetDistance2D(testPoint, segment));
		}

		[Fact()]
		public void CalculationsGetDistanceReturnsDistanceFromSegmentEndForPointProjetedOutsideSegment() {
			PointGeo orgin = new PointGeo(1, 1);
			PointGeo end = new PointGeo(2, 2);
			Segment<IPointGeo> segment = new Segment<IPointGeo>(orgin, end);

			PointGeo testPoint1 = new PointGeo(0, 0);
			PointGeo testPoint2 = new PointGeo(2, 3);

			Assert.Equal(Calculations.GetDistance2D(testPoint1, new PointGeo(1, 1)), Calculations.GetDistance2D(testPoint1, segment));
			Assert.Equal(Calculations.GetDistance2D(testPoint2, new PointGeo(2, 2)), Calculations.GetDistance2D(testPoint2, segment));
		}

		[Fact()]
		public void CalculationsGetDistanceReturnsDistanceFromPolyline() {
			PointGeo pt1 = new PointGeo(1, 1);
			PointGeo pt2 = new PointGeo(2, 2);
			PointGeo pt3 = new PointGeo(2, 3);

			Polyline<IPointGeo> line = new Polyline<IPointGeo>();
			line.Nodes.Add(pt1);
			line.Nodes.Add(pt2);
			line.Nodes.Add(pt3);

			PointGeo testPoint1 = new PointGeo(2, 1);
			PointGeo testPoint2 = new PointGeo(2, 2.5);
			PointGeo testPoint3 = new PointGeo(1, 3.5);

			Assert.Equal(Calculations.GetDistance2D(testPoint1, new PointGeo(1.5, 1.5)), Calculations.GetDistance2D(testPoint1, line));
			Assert.Equal(0, Calculations.GetDistance2D(testPoint2, line));
			Assert.Equal(Calculations.GetDistance2D(testPoint3, new PointGeo(2, 3)), Calculations.GetDistance2D(testPoint3, line));
		}
	}
}
