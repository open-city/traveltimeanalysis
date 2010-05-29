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
	}
}
