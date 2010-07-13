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
			TimeResolution target = new TimeResolution() { EpsMinutes = 60, Dates = DatesHandling.Days };

			Assert.True(target.AreClose(new DateTime(2010, 6, 30, 1, 0, 0), new DateTime(2010, 6, 30, 1, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithDaysParameterReturnsTrueForCloseTimesWithinSameWeekDay() {
			TimeResolution target = new TimeResolution() { EpsMinutes = 60, Dates = DatesHandling.Days };

			Assert.True(target.AreClose(new DateTime(2010, 6, 23, 1, 0, 0), new DateTime(2010, 6, 30, 1, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithDaysParameterReturnsFalseForNonCloseTimesWithinOneDay() {
			TimeResolution target = new TimeResolution() { EpsMinutes = 60, Dates = DatesHandling.Days };

			Assert.False(target.AreClose(new DateTime(2010, 6, 23, 1, 0, 0), new DateTime(2010, 6, 30, 2, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithDaysParameterReturnsFalseForCloseTimesWithinDifferentDay() {
			TimeResolution target = new TimeResolution() { EpsMinutes = 60, Dates = DatesHandling.Days };

			Assert.False(target.AreClose(new DateTime(2010, 6, 30, 1, 0, 0), new DateTime(2010, 6, 29, 1, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithWorkdayWeekendParameterReturnsTrueForCloseTimesWithinDayGroups() {
			TimeResolution target = new TimeResolution() { EpsMinutes = 60, Dates = DatesHandling.WeekendWorkdays };

			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 22, 1, 15, 0)));
			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 23, 1, 15, 0)));
			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 24, 1, 15, 0)));
			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 25, 1, 15, 0)));

			Assert.True(target.AreClose(new DateTime(2010, 6, 26, 1, 0, 0), new DateTime(2010, 6, 27, 1, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithWorkdayWeekendParameterReturnsTrueForCloseTimesAcrossMidnight() {
			TimeResolution target = new TimeResolution() { EpsMinutes = 60, Dates = DatesHandling.WeekendWorkdays };

			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 23, 45, 0), new DateTime(2010, 6, 22, 0, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithWorkdayWeekendParameterReturnsFalseForCloseAcrossDayGroups() {
			TimeResolution target = new TimeResolution() { EpsMinutes = 60, Dates = DatesHandling.WeekendWorkdays };

			Assert.False(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 26, 1, 15, 0)));
			Assert.False(target.AreClose(new DateTime(2010, 6, 21, 1, 0, 0), new DateTime(2010, 6, 27, 1, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithAnyParameterReturnsTrueForCloseTimes() {
			TimeResolution target = new TimeResolution() { EpsMinutes = 60, Dates = DatesHandling.Any };

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
			TimeResolution target = new TimeResolution() { EpsMinutes = 60, Dates = DatesHandling.Any };

			Assert.True(target.AreClose(new DateTime(2010, 6, 21, 23, 45, 0), new DateTime(2010, 6, 24, 0, 15, 0)));
		}

		[Fact()]
		public void AreCloseWithAnyParameterReturnsFalseForNonCloseTimes() {
			TimeResolution target = new TimeResolution() { EpsMinutes = 60, Dates = DatesHandling.WeekendWorkdays };

			Assert.False(target.AreClose(new DateTime(2010, 6, 21, 21, 45, 0), new DateTime(2010, 6, 22, 0, 15, 0)));
			Assert.False(target.AreClose(new DateTime(2010, 6, 22, 1, 45, 0), new DateTime(2010, 6, 22, 0, 15, 0)));
			Assert.False(target.AreClose(new DateTime(2010, 6, 25, 1, 45, 0), new DateTime(2010, 6, 22, 0, 15, 0)));
		}
	}
}
