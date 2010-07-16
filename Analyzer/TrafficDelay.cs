using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Analyzer {
	/// <summary>
	/// Describes the delay caused by congestions / heavy traffic
	/// </summary>
	public struct TrafficDelayInfo {
		/// <summary>
		/// Gets or sets day(s) when this traffic delay occurs
		/// </summary>
		public DayOfWeek AppliesTo { get; set; }

		/// <summary>
		/// Gets the time of the day, when this traffic delay starts
		/// </summary>
		public TimeSpan From { get; set; }

		/// <summary>
		/// Gets or sets the time of the day, when this traffic delay ends
		/// </summary>
		public TimeSpan To { get; set; }

		/// <summary>
		/// Gets or sets the length of the traffic delay in seconds
		/// </summary>
		public double Delay { get; set; }

		/// <summary>
		/// Returns string representation of the object
		/// </summary>
		/// <returns>The string representation of the object</returns>
		public override string ToString() {
			return string.Format("{0} {1}-{2} # {3}", AppliesTo, From, To, Delay);
		}


		public static IEnumerable<TrafficDelayInfo> Group(IEnumerable<TrafficDelayInfo> delays, int timeResolution) {


			double[] map = new double[24 * 60 / timeResolution];
			List<TrafficDelayInfo> result = new List<TrafficDelayInfo>();
			return result;
		}
	}

	/// <summary>
	/// Group and align multiple traffic delays
	/// </summary>
	public class TrafficDelayMap {
		private int _resolution;
		private double[,] _map;

		public TrafficDelayMap(int resolution) {
			if (24 * 60 % resolution != 0)
				throw new ArgumentException("Time resolution must divide 24 * 60");

			_resolution = resolution;
			_map = new double[7, 24 * 60 / resolution];
		}

		public void AddDelay(TimeSpan from, TimeSpan to, DayOfWeek day, double delay) {
			for (int i = 0; i < DayOfWeekFactory.Days.Length; i++) {
				if ((day & DayOfWeekFactory.Days[i]) > 0) {
					int indexFrom = (int)from.TotalMinutes / _resolution;
					int indexTo = (int)to.TotalMinutes / _resolution;

					for (int ii = indexFrom; ii <= indexTo; ii++) {
						_map[i, ii] = delay;
					}
				}
			}
		}

		public IEnumerable<TrafficDelayInfo> GetDelays() {
			List<TrafficDelayInfo> result = new List<TrafficDelayInfo>();

			int dayIndex = 0;
			int timeIndex = 0;

			while (MapIsEmpty(ref dayIndex, ref timeIndex) == false) {
				TrafficDelayInfo delay = new TrafficDelayInfo();
				delay.Delay = _map[dayIndex, timeIndex];
				delay.From = new TimeSpan(0, timeIndex * _resolution, 0);

				for (int i = 0; i < 7; i++) {
					if (_map[i, timeIndex] == delay.Delay)
						delay.AppliesTo |= DayOfWeekFactory.Days[i];
				}
				DayOfWeek timeBinDays = delay.AppliesTo;

				while (timeIndex < 24 * 60 / _resolution && timeBinDays == delay.AppliesTo) {
					for (int i = 0; i < 7; i++) {
						if ((timeBinDays & DayOfWeekFactory.Days[i]) > 0)
							_map[i, timeIndex] = 0;
					}
					
					timeIndex++;

					timeBinDays = 0;
					for (int i = 0; i < 7; i++) {
						if (_map[i, timeIndex] == delay.Delay)
							timeBinDays |= DayOfWeekFactory.Days[i];
					}
				}

				delay.To = new TimeSpan(0, timeIndex * _resolution, 0);
				result.Add(delay);
			}

			return result;
		}

		bool MapIsEmpty(ref int dayIndex, ref int timeIndex) {
			for (int ti = 0; ti < 24 * 60 / _resolution; ti++) {
				for (int di = 0; di < 7; di++) {
					if (_map[di, ti] > 0) {
						dayIndex = di;
						timeIndex = ti;
						return false;
					}
				}
			}

			return true;
		}
	}
}
