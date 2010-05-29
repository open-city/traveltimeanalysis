using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GeoUtils;
using LK.GeoUtils.Geometry;

namespace GeoUtils.Tests {
	public class GreatCircleDistanceCalculatorTest {
		[Fact()]
		public void GreatCircleDistanceCalculatorCalculate2DReturnsCorrectValuesForTestCases() {
			GreatCircleDistanceCalculator target = new GreatCircleDistanceCalculator();

			double error = Math.Abs(target.Calculate2D(new PointGeo(36.12, -86.67), new PointGeo(33.94, -118.4)) - 2886449);
			Assert.True(error < 1);
		}
	}
}
