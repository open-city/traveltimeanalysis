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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.OSM2Routing;
using LK.OSMUtils.OSMDatabase;

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
