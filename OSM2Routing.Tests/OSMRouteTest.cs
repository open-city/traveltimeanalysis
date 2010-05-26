using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.OSMUtils.OSMDatabase;
using LK.OSM2Routing;

namespace OSM2Routing.Tests {
	public class OSMRouteTest {
		[Fact()]
		public void OSMRouteConstructorSetsId() {
			OSMRoute target = new OSMRoute(11);

			Assert.Equal(11, target.ID);
		}

		[Fact()]
		public void OSMRouteConstructorCopiesDataFromWay() {
			OSMWay source = new OSMWay(11);
			source.Nodes.Add(1);
			source.Nodes.Add(2);
			source.Tags.Add(new OSMTag("highway", "track"));

			RoadType sourceType = new RoadType();
			sourceType.RequiredTags.Add(new OSMTag("highway", "track"));

			OSMRoute target = new OSMRoute(source, sourceType);

			Assert.Equal(source.Nodes.Count, target.Nodes.Count);
			Assert.Equal(source.Tags.First(), target.Tags.First());
			Assert.Equal(sourceType, target.RoadType);
		}
	}
}
