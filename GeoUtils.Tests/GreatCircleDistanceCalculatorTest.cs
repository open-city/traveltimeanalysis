//  Travel Time Analysis project
//  Copyright (C) 2010 Lukas Kabrt
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
