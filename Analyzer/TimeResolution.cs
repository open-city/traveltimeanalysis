using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Analyzer {

	public struct TimeResolution {
		public int EpsMinutes;
		public DatesHandling Dates;

		public bool AreClose(DateTime item1, DateTime item2) {
			double minutes = Math.Abs((item1.TimeOfDay - item2.TimeOfDay).TotalMinutes);
			DayOfWeek item1Day = DayOfWeekHelper.FromDate(item1);
			DayOfWeek item2Day = DayOfWeekHelper.FromDate(item2);

			if (Dates == DatesHandling.Days) {
				return (item1.DayOfWeek == item2.DayOfWeek) && (minutes < EpsMinutes);
			}
			else if (Dates == DatesHandling.WeekendWorkdays) {
				if (minutes > 12 * 60)
					minutes = 24 * 60 - minutes;

				return (minutes < EpsMinutes) &&
							 (((DayOfWeek.Workday & item1Day) > 0 && ((DayOfWeek.Workday & item2Day) > 0)) || ((DayOfWeek.Weekend & item1Day) > 0 && ((DayOfWeek.Weekend & item2Day) > 0)));
			}
			else {
				if (minutes > 12 * 60)
					minutes = 24 * 60 - minutes;

				return (minutes < EpsMinutes);
			}
		}
	}

	public enum DatesHandling {
		Days,
		WeekendWorkdays,
		Any
	}
}
