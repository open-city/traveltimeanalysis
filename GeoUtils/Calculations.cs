using System;
using System.Collections.Generic;
using System.Text;

using LK.GeoUtils.Geometry;
using System.Collections;

namespace LK.GeoUtils {
	/// <summary>
	/// Encapsulates various geographic calculations
	/// </summary>
	public static class Calculations {
		static IDistanceCalculator _distanceCalculator;

		/// <summary>
		/// Initializes static class
		/// </summary>
		static Calculations() {
			_distanceCalculator = new GreatCircleDistanceCalculator();
		}

		/// <summary>
		/// Calculates distance between 2 points on geoid surface (ignores points elevations)
		/// </summary>
		/// <param name="pt1">First point</param>
		/// <param name="pt2">Second point</param>
		/// <returns>distance between given points in meters</returns>
		public static double GetDistance2D(IPointGeo pt1, IPointGeo pt2) {
			return _distanceCalculator.Calculate2D(pt1, pt2);
		}

		/// <summary>
		/// Calculates length of the specific polyline
		/// </summary>
		/// <param name="line">The line to be measured</param>
		/// <returns>length of the line in meters</returns>
		public static double GetLength(IPolyline line) {
			double length = 0;
			
			for (int i = 0; i < line.NodesCount -1; i++) {
				length += Calculations.GetDistance2D(line.Nodes[i], line.Nodes[i + 1]);				
			}

			return length;
		}

		/// <summary>
		/// Calculate average position from series of points
		/// </summary>
		/// <param name="point">The collection of points used to compute average position</param>
		/// <returns>The average position of points</returns>
		public static PointGeo Averaging(IEnumerable points) {
			int count = 0;
			double lat = 0, lon = 0;

			foreach (IPointGeo point in points) {
				lat += point.Latitude;
				lon += point.Longitude;
				count++;
			}

			return new PointGeo(lat / count, lon / count);
		}
	}
}
