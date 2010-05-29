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
		public void RoutingConfigLoadThrowsExceptionForConfigWithOtherVersionThan1() {
			RoutingConfig target = new RoutingConfig();

			Assert.Throws<XmlException>(delegate { target.Load(new MemoryStream(TestData.config_wrong_version)); });
		}

		[Fact()]
		public void RoutingConfigLoadThrowsExceptionForConfigWithMissingVersionAttribute() {
			RoutingConfig target = new RoutingConfig();

			Assert.Throws<XmlException>(delegate { target.Load(new MemoryStream(TestData.config_missing_version)); });
		}

		[Fact()]
		public void RoutingConfigLoadReadsRoadTypes() {
			RoutingConfig target = new RoutingConfig();

			//  <route-type name="residental"  speed="50">
			//    <required-tag key="highway" value="residental" />
			//  </route-type>
			//  <route-type name="bad-track"  speed="20">
			//    <required-tag key="highway" value="track" />
			//    <required-tag key="grade" value="5" />
			//  </route-type>

			target.Load(new MemoryStream(TestData.config_road_types));

			Assert.Equal(2, target.RoadTypes.Count);

			Assert.Equal("residental", target.RoadTypes[0].Name);
			Assert.Equal(50, target.RoadTypes[0].Speed);
			Assert.Contains(new OSMTag("highway", "residental"), target.RoadTypes[0].RequiredTags);

			Assert.Equal("bad-track", target.RoadTypes[1].Name);
			Assert.Equal(20, target.RoadTypes[1].Speed);
			Assert.Contains(new OSMTag("highway", "track"), target.RoadTypes[1].RequiredTags);
			Assert.Contains(new OSMTag("grade", "5"), target.RoadTypes[1].RequiredTags);
		}

		[Fact()]
		public void RoutingConfigLoadReadsOptionalOnewayAttribute() {
			RoutingConfig target = new RoutingConfig();

			//  <route-type name="residental" speed="50">
			//    <required-tag key="highway" value="residental" />
			//  </route-type>
	
			//  <route-type name="trunk"  speed="20" oneway="yes">
			//    <required-tag key="highway" value="trunk" />
			//  </route-type>

			//  <route-type name="trunk"  speed="20" oneway="no">
			//    <required-tag key="highway" value="trunk" />
			//  </route-type>

			//  <route-type name="trunk"  speed="20" oneway="other-value">
			//    <required-tag key="highway" value="trunk" />
			//  </route-type>

			target.Load(new MemoryStream(TestData.config_oneway));

			Assert.Equal(4, target.RoadTypes.Count);
			Assert.Equal(false, target.RoadTypes[0].Oneway);
			Assert.Equal(true, target.RoadTypes[1].Oneway);
			Assert.Equal(false, target.RoadTypes[2].Oneway);
			Assert.Equal(false, target.RoadTypes[3].Oneway);
		}
	}
}
