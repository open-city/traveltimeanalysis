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
