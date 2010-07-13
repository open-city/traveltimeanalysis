using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Analyzer {
	public class Model {
		public SegmentInfo Segment { get; set; }

		public double FreeFlowTravelTime { get; set; }
		public List<TrafficDelayInfo> TrafficDelay { get; private set; }
		public TrafficSignalsDelayInfo TrafficSignalsDelay { get; set; }

		public Model() {
			TrafficDelay = new List<TrafficDelayInfo>();
		}
	}

	public struct TrafficSignalsDelayInfo {
		public double Length;
		public double Probability;
	}

	[Flags()]
	public enum DayOfWeek {
		Monday = 1,
		Tuesday = 2,
		Wednesday = 4,
		Thursday = 8,
		Friday = 16,
		Saturday = 32,
		Sunday = 64,

		Weekend = Saturday | Sunday,
		Workday = Monday | Tuesday | Wednesday | Thursday | Friday,

		Any = Weekend | Workday
	}

	public static class DayOfWeekFactory {
		public static DayOfWeek FromDate(DateTime date) {
			switch (date.DayOfWeek) {
				case System.DayOfWeek.Monday: return DayOfWeek.Monday;
				case System.DayOfWeek.Tuesday: return DayOfWeek.Tuesday;
				case System.DayOfWeek.Wednesday: return DayOfWeek.Wednesday;
				case System.DayOfWeek.Thursday: return DayOfWeek.Thursday;
				case System.DayOfWeek.Friday: return DayOfWeek.Friday;
				case System.DayOfWeek.Saturday: return DayOfWeek.Saturday;
				case System.DayOfWeek.Sunday: return DayOfWeek.Sunday;
				default: return DayOfWeek.Any;
			}
		}
	}

	public struct TrafficDelayInfo {
		public DayOfWeek Day { get; set; }
		public TimeSpan From { get; set; }
		public TimeSpan To { get; set; }
		public double Delay { get; set; }
	}
}
