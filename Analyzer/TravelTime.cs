using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.MatchGPX2OSM;
using LK.GPXUtils;

namespace LK.Analyzer {
	/// <summary>
	/// Represents travel time between two points
	/// </summary>
	public class TravelTime {
		/// <summary>
		/// Gets total travel time between Start and End point
		/// </summary>
		public TimeSpan TotalTravelTime {
			get {
				return TimeEnd - TimeStart;
			}
		}

		private DateTime _timeStart;
		/// <summary>
		/// Gets DateTime at the Start point
		/// </summary>
		public DateTime TimeStart {
			get {
				return _timeStart;
			}
		}

		private DateTime _timeEnd;
		/// <summary>
		/// Gets DateTime at the End point
		/// </summary>
		public DateTime TimeEnd {
			get {
				return _timeEnd;
			}
		}

		private List<GPXPoint> _intermediatePoints;
		/// <summary>
		/// Gets the list of intermediate points (with the time) between start and end point
		/// </summary>
		public IList<GPXPoint> IntermediatePoints {
			get {
				return _intermediatePoints;
			}
		}

		/// <summary>
		/// Creates a new instance of the TravelTime object
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="points"></param>
		public TravelTime(DateTime start, DateTime end, IList<GPXPoint> points) {
			_timeStart = start;
			_timeEnd = end;

			_intermediatePoints = new List<GPXPoint>();
			_intermediatePoints.AddRange(points);
		}
	}
}
