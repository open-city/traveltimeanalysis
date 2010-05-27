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

		[Fact()]
		public void RoadTypeIsAccessibleAndIsAccessibleReturnsCorrectValuesForNonOnewayRoads() {
			RoadType roadType = new RoadType();
			roadType.RequiredTags.Add(new OSMTag("highway", "*"));

			OSMWay oneWay1 = new OSMWay(0);
			oneWay1.Tags.Add(new OSMTag("oneway", "no"));
			OSMRoad target1 = new OSMRoad(oneWay1, roadType);

			OSMWay oneWay2 = new OSMWay(0);
			oneWay2.Tags.Add(new OSMTag("oneway", "0"));
			OSMRoad target2 = new OSMRoad(oneWay2, roadType);

			OSMWay oneWay3 = new OSMWay(0);
			oneWay3.Tags.Add(new OSMTag("oneway", "false"));
			OSMRoad target3 = new OSMRoad(oneWay3, roadType);

			OSMWay oneWay4 = new OSMWay(0);
			OSMRoad target4 = new OSMRoad(oneWay4, roadType);

			Assert.True(target1.IsAccessible());
			Assert.True(target1.IsAccessibleReverse());

			Assert.True(target2.IsAccessible());
			Assert.True(target2.IsAccessibleReverse());

			Assert.True(target3.IsAccessible());
			Assert.True(target3.IsAccessibleReverse());

			Assert.True(target4.IsAccessible());
			Assert.True(target4.IsAccessibleReverse());
		}

		[Fact()]
		public void RoadTypeIsAccessibleAndIsAccessibleReturnsCorrectValuesForOnewayRoads() {
			RoadType roadType = new RoadType();
			roadType.RequiredTags.Add(new OSMTag("highway", "*"));

			OSMWay oneWay1 = new OSMWay(0);
			oneWay1.Tags.Add(new OSMTag("oneway", "yes"));
			OSMRoad target1 = new OSMRoad(oneWay1, roadType);

			OSMWay oneWay2 = new OSMWay(0);
			oneWay2.Tags.Add(new OSMTag("oneway", "1"));
			OSMRoad target2 = new OSMRoad(oneWay2, roadType);

			OSMWay oneWay3 = new OSMWay(0);
			oneWay3.Tags.Add(new OSMTag("oneway", "true"));
			OSMRoad target3 = new OSMRoad(oneWay3, roadType);


			Assert.True(target1.IsAccessible());
			Assert.False(target1.IsAccessibleReverse());

			Assert.True(target2.IsAccessible());
			Assert.False(target2.IsAccessibleReverse());

			Assert.True(target3.IsAccessible());
			Assert.False(target3.IsAccessibleReverse());
		}

		[Fact()]
		public void RoadTypeIsAccessibleAndIsAccessibleReturnsCorrectValuesForReverseOnewayRoads() {
			RoadType roadType = new RoadType();
			roadType.RequiredTags.Add(new OSMTag("highway", "*"));

			OSMWay oneWay1 = new OSMWay(0);
			oneWay1.Tags.Add(new OSMTag("oneway", "-1"));
			OSMRoad target1 = new OSMRoad(oneWay1, roadType);

			OSMWay oneWay2 = new OSMWay(0);
			oneWay2.Tags.Add(new OSMTag("oneway", "reverse"));
			OSMRoad target2 = new OSMRoad(oneWay2, roadType);

			Assert.False(target1.IsAccessible());
			Assert.True(target1.IsAccessibleReverse());

			Assert.False(target2.IsAccessible());
			Assert.True(target2.IsAccessibleReverse());
		}
	}
}
