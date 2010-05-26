using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.OSM2Routing;
using LK.OSMUtils.OSMDatabase;

using Xunit;

namespace OSM2Routing.Tests {
	public class RoadTypeTest {
		[Fact()]
		public void RoadTypeConstructorInitializesProperties() {
			RoadType target = new RoadType();

			Assert.NotNull(target.RequiredTags);
		}

		[Fact()]
		public void RoadTypeMatchReturnsTrueForTheSameTags() {
			RoadType target = new RoadType();
			target.RequiredTags.Add(new OSMTag("highway", "residental"));

			OSMWay testObject = new OSMWay(1);
			testObject.Tags.Add(new OSMTag("highway", "residental"));

			Assert.True(target.Match(testObject));
		}

		[Fact()]
		public void RoadTypeMatchReturnsTrueForTheSameTagKeyAndWildcardValue() {
			RoadType target = new RoadType();
			target.RequiredTags.Add(new OSMTag("highway", "*"));

			OSMWay testObject = new OSMWay(1);
			testObject.Tags.Add(new OSMTag("highway", "residental"));

			Assert.True(target.Match(testObject));
		}

		[Fact()]
		public void RoadTypeMatchReturnsTrueForTheSameMultipleTags() {
			RoadType target = new RoadType();
			target.RequiredTags.Add(new OSMTag("highway", "track"));
			target.RequiredTags.Add(new OSMTag("grade", "1"));

			OSMWay testObject = new OSMWay(1);
			testObject.Tags.Add(new OSMTag("grade", "1"));
			testObject.Tags.Add(new OSMTag("highway", "track"));

			Assert.True(target.Match(testObject));
		}

		[Fact()]
		public void RoadTypeMatchReturnsFalseForMissingTag() {
			RoadType target = new RoadType();
			target.RequiredTags.Add(new OSMTag("highway", "residental"));

			OSMWay testObject = new OSMWay(1);

			Assert.False(target.Match(testObject));
		}

		[Fact()]
		public void RoadTypeMatchReturnsFalseForTagWithDifferentValue() {
			RoadType target = new RoadType();
			target.RequiredTags.Add(new OSMTag("highway", "residental"));

			OSMWay testObject = new OSMWay(1);
			testObject.Tags.Add(new OSMTag("highway", "primary"));

			Assert.False(target.Match(testObject));
		}

		[Fact()]
		public void RoadTypeMatchReturnsFalseForMultipleTagsWithDifferentValues() {
			RoadType target = new RoadType();
			target.RequiredTags.Add(new OSMTag("highway", "track"));
			target.RequiredTags.Add(new OSMTag("grade", "1"));

			OSMWay testObject1 = new OSMWay(1);
			testObject1.Tags.Add(new OSMTag("highway", "primary"));
			target.RequiredTags.Add(new OSMTag("grade", "1"));

			OSMWay testObject2 = new OSMWay(1);
			testObject2.Tags.Add(new OSMTag("highway", "track"));
			target.RequiredTags.Add(new OSMTag("grade", "2"));

			OSMWay testObject3 = new OSMWay(1);
			testObject3.Tags.Add(new OSMTag("highway", "track"));

			Assert.False(target.Match(testObject1));
			Assert.False(target.Match(testObject2));
			Assert.False(target.Match(testObject3));
		}
	}
}
