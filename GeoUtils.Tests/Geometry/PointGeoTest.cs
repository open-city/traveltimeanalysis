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

using LK.GeoUtils.Geometry;
using System;
using Xunit;

namespace GeoUtils.Tests
{
    
    
    /// <summary>
    ///This is a test class for PointGeoTest and is intended
    ///to contain all PointGeoTest Unit Tests
    ///</summary>
	
	public class PointGeoTest {
		[Fact()]
		public void PointGeoConstructorSetsLatitudeAndLongitude() {
			double lat = 10.1;
			double lon = 12.8;

			PointGeo target = new PointGeo(lat, lon);

			Assert.Equal(lat, target.Latitude);
			Assert.Equal(lon, target.Longitude);
		}

		public void PointGeoConstructorSetsLatitudeLongitudeAndElevation() {
			double lat = 10.1;
			double lon = 12.8;
			double elevation = 120;

			PointGeo target = new PointGeo(lat, lon, elevation);

			Assert.Equal(lat, target.Latitude);
			Assert.Equal(lon, target.Longitude);
			Assert.Equal(elevation, target.Elevation);
		}
	}
}
