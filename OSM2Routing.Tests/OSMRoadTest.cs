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

using LK.OSMUtils.OSMDatabase;
using LK.OSM2Routing;

namespace OSM2Routing.Tests {
	public class OSMRoadTest {
		[Fact()]
		public void OSMRouteConstructorSetsId() {
			OSMRoad target = new OSMRoad(11);

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

			OSMRoad target = new OSMRoad(source, sourceType);

			Assert.Equal(source.Nodes.Count, target.Nodes.Count);
			Assert.Equal(source.Tags.First(), target.Tags.First());
			Assert.Equal(sourceType, target.RoadType);
		}

		[Fact()]
		public void OSMRouteConstructorSetsSpeedPropertyFromRoadTypeSpeed() {
			OSMWay source = new OSMWay(11);

			RoadType sourceType = new RoadType();
			sourceType.Speed = 60;

			OSMRoad target = new OSMRoad(source, sourceType);

			Assert.Equal(60, target.Speed);
		}

		[Fact()]
		public void OSMRouteConstructorSetsSpeedPropertyFromMaxSpeedTag() {
			OSMWay source = new OSMWay(11);
			source.Tags.Add(new OSMTag("maxspeed", "50"));

			RoadType sourceType = new RoadType();

			OSMRoad target = new OSMRoad(source, sourceType);

			Assert.Equal(50, target.Speed);
		}

		[Fact()]
		public void OSMRouteConstructorSetsSpeedPropertyConvertsFromMph() {
			OSMWay source = new OSMWay(11);
			source.Tags.Add(new OSMTag("maxspeed", "50 mph"));

			RoadType sourceType = new RoadType();

			OSMRoad target = new OSMRoad(source, sourceType);

			Assert.Equal(50 * 1.609, target.Speed);
		}

		[Fact()]
		public void OSMRouteConstructorSetsSpeedPropertyMaxspeedTagTakesPrecedenceOverRoadTypeSpeed() {
			OSMWay source = new OSMWay(11);
			source.Tags.Add(new OSMTag("maxspeed", "50"));

			RoadType sourceType = new RoadType();
			sourceType.Speed = 60;

			OSMRoad target = new OSMRoad(source, sourceType);

			Assert.Equal(50, target.Speed);
		}

		[Fact()]
		public void OSMRouteConstructorSetsSpeedPropertyIgnoresInvalidMaxspeedTags() {
			OSMWay source = new OSMWay(11);
			source.Tags.Add(new OSMTag("maxspeed", "some value"));

			RoadType sourceType = new RoadType();
			sourceType.Speed = 60;

			OSMRoad target = new OSMRoad(source, sourceType);

			Assert.Equal(60, target.Speed);
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

		[Fact()]
		public void RoadTypeIsAccessibleReverseAppliesDefaultOnewayValueFromRoadType() {
			RoadType roadType = new RoadType();
			roadType.Oneway = true;

			OSMWay oneWay = new OSMWay(0);
			OSMRoad target = new OSMRoad(oneWay, roadType);

			Assert.Equal(true, target.IsAccessible());
			Assert.Equal(false, target.IsAccessibleReverse());
		}

		[Fact()]
		public void RoadTypeIsAccessibleReverseAppliesOnewayTagOverridesDefaultValueForRoadType() {
			RoadType roadType = new RoadType();
			roadType.Oneway = true;

			OSMWay way = new OSMWay(0);
			way.Tags.Add(new OSMTag("oneway", "no"));

			OSMRoad target = new OSMRoad(way, roadType);

			Assert.Equal(true, target.IsAccessible());
			Assert.Equal(true, target.IsAccessibleReverse());
		}

		[Fact()]
		public void RoadTypeIsAccessibleReverseAppliesOnewayTagOverridesDefaultValueForRoadType2() {
			RoadType roadType = new RoadType();
			roadType.Oneway = false;

			OSMWay way = new OSMWay(0);
			way.Tags.Add(new OSMTag("oneway", "yes"));

			OSMRoad target = new OSMRoad(way, roadType);

			Assert.Equal(true, target.IsAccessible());
			Assert.Equal(false, target.IsAccessibleReverse());
		}
	}
}
