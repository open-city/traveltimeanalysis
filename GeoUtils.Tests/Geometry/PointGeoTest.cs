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
