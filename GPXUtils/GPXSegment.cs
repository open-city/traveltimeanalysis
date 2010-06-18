using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.GPXUtils {
	/// <summary>
	/// Represents a segment of the GPS track between two points
	/// </summary>
	public class GPXSegment : Segment<GPXPoint> {
		/// <summary>
		/// Creates a new segment with the specific start and end points
		/// </summary>
		/// <param name="start">The point, where segment starts</param>
		/// <param name="end">The point, where segment ends</param>
		public GPXSegment(GPXPoint start, GPXPoint end)
			: base(start, end) {
		}

		/// <summary>
		/// Gets the time of travel between the start and the end point 
		/// </summary>
		public TimeSpan TravelTime {
			get {
				return EndPoint.Time - StartPoint.Time;
			}
		}

		/// <summary>
		/// Gets the average speed on the segment in kph
		/// </summary>
		public double AverageSpeed {
			get {
				return Length / 1000 / TravelTime.TotalHours;
			}
		}
	}
}
