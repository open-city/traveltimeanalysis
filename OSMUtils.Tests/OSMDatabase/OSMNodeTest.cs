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

using LK.OSMUtils.OSMDatabase;
using Xunit;
using System;

using LK.GeoUtils.Geometry;

namespace OSMUtils.Tests
{
    
    
    /// <summary>
    ///This is a test class for OSMNodeTest and is intended
    ///to contain all OSMNodeTest Unit Tests
    ///</summary>
	
	public class OSMNodeTest {
		[Fact()]
		public void OSMNodeConstructorSetsAllProperties() {
			int id = 1374;
			double latitude = 23.4;
			double longitude = 12.9;

			OSMNode target = new OSMNode(id, latitude, longitude);

			Assert.Equal(id, target.ID);
			Assert.Equal(latitude, target.Latitude);
			Assert.Equal(longitude, target.Longitude);
		}

		[Fact()]
		public void OSMNodeLatitudePropertyGetsAndReturnsCorrectValues() {
			int id = 1374;
			OSMNode target = new OSMNode(id, 0, 0);
			double expected = 23.4;
			
			target.Latitude = expected;

			Assert.Equal(expected, target.Latitude);
		}

		[Fact()]
		public void OSMNodeLongitudePropertyGetsAndReturnsCorrectValues() {
			int id = 1374;
			OSMNode target = new OSMNode(id, 0, 0);
			double expected = 12.9;

			target.Longitude = expected;

			Assert.Equal(expected, target.Longitude);
		}

		[Fact()]
		public void OSMNodeImplementsIPointGeo() {
			int id = 1374;
			IPointGeo target = new OSMNode(id, 12.9, 13.9);

			Assert.Equal(12.9, target.Latitude);
			Assert.Equal(13.9, target.Longitude);
			Assert.Equal(0, target.Elevation);
		}
	}
}
