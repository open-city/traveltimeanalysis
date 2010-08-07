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
