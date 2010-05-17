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
