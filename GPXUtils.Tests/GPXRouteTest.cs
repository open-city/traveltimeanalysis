using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GPXUtils;
using LK.GPXUtils.GPXDataSource;

namespace GPXUtils.Tests {
	public class GPXRouteTest {
		[Fact()]
		public void GPXRouteImplementsIPolyline() {
			GPXRoute target = new GPXRoute();

			LK.GeoUtils.Geometry.IPolyline<GPXPoint> castedTarget = target as LK.GeoUtils.Geometry.IPolyline<GPXPoint>;

			Assert.NotNull(castedTarget);
		}

		[Fact()]
		public void GPXRouteConstructorCreatesRouteWithName() {
			GPXRoute target = new GPXRoute("ROUTE");

			Assert.Equal("ROUTE", target.Name);
		}
	}
}
