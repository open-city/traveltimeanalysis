using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GPXUtils;

namespace LK.Analyzer {
	/// <summary>
	/// Represents travel time between two points
	/// </summary>
	public class TravelTime {
		private SegmentInfo _segment;
		/// <summary>
		/// Gets the segment that is associated with this TravelTime object
		/// </summary>
		public SegmentInfo Segment {
			get { return _segment; }
		}

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

		private List<GPXPoint> _points;
		/// <summary>
		/// Gets the list of points (with the time) on the associate segment
		/// </summary>
		public IList<GPXPoint> Points {
			get {
				return _points;
			}
		}

		/// <summary>
		/// Creates a new instance of the TravelTime object
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="points"></param>
		public TravelTime(SegmentInfo segment, DateTime start, DateTime end, IList<GPXPoint> points) {
			_segment = segment;
			_timeStart = start;
			_timeEnd = end;

			_points = new List<GPXPoint>();
			_points.AddRange(points);
		}
	}
}
