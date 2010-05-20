using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.GPXUtils {
	/// <summary>
	/// Represents a list of GPXPoints which are logically connected in order.
	/// </summary>
	public class GPXTrackSegment : Polyline<GPXPoint> {
		/// <summary>
		/// Creates a new, empty instance of GPXTrackSegment 
		/// </summary>
		public GPXTrackSegment()
			: base() {
		}

		/// <summary>
		/// Creates a new instance of GPXTrackSegment and initializes it with specific collection of GPXPoints
		/// </summary>
		/// <param name="points">Points that belong to the GPXTrackSegment</param>
		public GPXTrackSegment(IEnumerable<GPXPoint> points)
			: base() {

				_nodes.AddRange(points);
		}
	}
}
