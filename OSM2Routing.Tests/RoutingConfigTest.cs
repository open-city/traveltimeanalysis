using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.OSM2Routing;
using LK.OSMUtils.OSMDatabase;

using System.Xml;
using System.IO;

namespace OSM2Routing.Tests {
	public class RoutingConfigTest {
		[Fact()]
		public void RoutingConfigContructorInitializesFields() {
			RoutingConfig target = new RoutingConfig();

			Assert.NotNull(target.RoadTypes);
		}

		[Fact()]
		public void RoutingConfigLoadThrowsExceptionForConfigWithWrongRootElement() {
			RoutingConfig target = new RoutingConfig();

			Assert.Throws<XmlException>(delegate { target.Load(new MemoryStream(TestData.config_wrong_root_element)); });
		}

		[Fact()]
		public void RoutingConfigLoadReadsRoadTypes() {
			RoutingConfig target = new RoutingConfig();

			//  <route-type name="residental>
			//    <required-tag key="highway" value="residental" />
			//  </route-type>
			//  <route-type name="bad-track>
			//    <required-tag key="highway" value="track" />
			//    <required-tag key="grade" value="5" />
			//  </route-type>

			target.Load(new MemoryStream(TestData.config_road_types));

			Assert.Equal(2, target.RoadTypes.Count);

			Assert.Equal("residental", target.RoadTypes[0].Name);
			Assert.Contains(new OSMTag("highway", "residental"), target.RoadTypes[0].RequiredTags);

			Assert.Equal("bad-track", target.RoadTypes[1].Name);
			Assert.Contains(new OSMTag("highway", "track"), target.RoadTypes[1].RequiredTags);
			Assert.Contains(new OSMTag("grade", "5"), target.RoadTypes[1].RequiredTags);
		}
	}
}
