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
	public class GPXPointTest {
		[Fact()]
		public void PointGeoConstructorSetsLatitudeAndLongitude() {
			double lat = 10.1;
			double lon = 12.8;

			GPXPoint target = new GPXPoint(lat, lon);

			Assert.Equal(lat, target.Latitude);
			Assert.Equal(lon, target.Longitude);
		}

		[Fact()]
		public void PointGeoConstructorSetsLatitudeLongitudeAndTime() {
			double lat = 10.1;
			double lon = 12.8;
			DateTime time = new DateTime(2010, 5, 1, 13, 10, 1);

			GPXPoint target = new GPXPoint(lat, lon, time);

			Assert.Equal(lat, target.Latitude);
			Assert.Equal(lon, target.Longitude);
			Assert.Equal(time, target.Time);
		}

		[Fact()]
		public void PointGeoConstructorSetsLatitudeLongitudeTimeAndElevation() {
			double lat = 10.1;
			double lon = 12.8;
			DateTime time = new DateTime(2010, 5, 1, 13, 10, 1);
			double ele = 135.8;

			GPXPoint target = new GPXPoint(lat, lon, time, ele);

			Assert.Equal(lat, target.Latitude);
			Assert.Equal(lon, target.Longitude);
			Assert.Equal(time, target.Time);
			Assert.Equal(ele, target.Elevation);
		}
	}
}
