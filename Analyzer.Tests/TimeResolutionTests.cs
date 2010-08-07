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

using LK.Analyzer;

namespace Analyzer.Tests {
	public class TimeResolutionTests {
		[Fact()]
		public void AreCloseWithDaysParameterReturnsTrueForCloseTimesWithinOneDay() {
			ClusterParameters target = new ClusterParameters() { MembersTimeDifference = 60, Dates = DatesHandling.Days };

			Assert.True(target.AreClose(new DateTime(2010, 6, 30, 1, 0, 0), new DateTime(2010, 6, 30, 1, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithDaysParameterReturnsTrueForCloseTimesWithinSameWeekDay() {
			ClusterParameters target = new ClusterParameters() { MembersTimeDifference = 60, Dates = DatesHandling.Days };

			Assert.True(target.AreClose(new DateTime(2010, 6, 23, 1, 0, 0), new DateTime(2010, 6, 30, 1, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithDaysParameterReturnsFalseForNonCloseTimesWithinOneDay() {
			ClusterParameters target = new ClusterParameters() { MembersTimeDifference = 60, Dates = DatesHandling.Days };

			Assert.False(target.AreClose(new DateTime(2010, 6, 23, 1, 0, 0), new DateTime(2010, 6, 30, 2, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithDaysParameterReturnsFalseForCloseTimesWithinDifferentDay() {
			ClusterParameters target = new ClusterParameters() { MembersTimeDifference = 60, Dates = DatesHandling.Days };

			Assert.False(target.AreClose(new DateTime(2010, 6, 30, 1, 0, 0), new DateTime(2010, 6, 29, 1, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithWorkdayWeekendParameterReturnsTrueForCloseTimesWithinDayGroups() {
			ClusterParameters target = new ClusterParameters() { MembersTimeDifference = 60, Dates = DatesHandling.WeekendWorkdays };

			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 22, 1, 15, 0)));
			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 23, 1, 15, 0)));
			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 24, 1, 15, 0)));
			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 25, 1, 15, 0)));

			Assert.True(target.AreClose(new DateTime(2010, 6, 26, 1, 0, 0), new DateTime(2010, 6, 27, 1, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithWorkdayWeekendParameterReturnsTrueForCloseTimesAcrossMidnight() {
			ClusterParameters target = new ClusterParameters() { MembersTimeDifference = 60, Dates = DatesHandling.WeekendWorkdays };

			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 23, 45, 0), new DateTime(2010, 6, 22, 0, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithWorkdayWeekendParameterReturnsFalseForCloseAcrossDayGroups() {
			ClusterParameters target = new ClusterParameters() { MembersTimeDifference = 60, Dates = DatesHandling.WeekendWorkdays };

			Assert.False(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 26, 1, 15, 0)));
			Assert.False(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 27, 1, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithAnyParameterReturnsTrueForCloseTimes() {
			ClusterParameters target = new ClusterParameters() { MembersTimeDifference = 60, Dates = DatesHandling.Any };

			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 22, 1, 15, 0)));
			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 23, 1, 15, 0)));
			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 24, 1, 15, 0)));
			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 25, 1, 15, 0)));
			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 26, 1, 15, 0)));
			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 27, 1, 15, 0)));

			Assert.True(target.AreClose(new DateTime(2019, 6, 21, 1, 0, 0), new DateTime(2010, 6, 27, 1, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithAnyParameterReturnsTrueForCloseTimesAcrossMidnight() {
			ClusterParameters target = new ClusterParameters() { MembersTimeDifference = 60, Dates = DatesHandling.Any };

			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 23, 45, 0), new DateTime(2010, 6, 24, 0, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithAnyParameterReturnsFalseForNonCloseTimes() {
			ClusterParameters target = new ClusterParameters() { MembersTimeDifference = 60, Dates = DatesHandling.WeekendWorkdays };

			Assert.False(target.AreClose(new DateTime(2010, 6, 21, 21, 45, 0), new DateTime(2010, 6, 22, 0, 15, 0)));
			Assert.False(target.AreClose(new DateTime(2010, 6, 22, 1, 45, 0), new DateTime(2010, 6, 22, 0, 15, 0)));
			Assert.False(target.AreClose(new DateTime(2010, 6, 25, 1, 45, 0), new DateTime(2010, 6, 22, 0, 15, 0)));
		}
	}
}
