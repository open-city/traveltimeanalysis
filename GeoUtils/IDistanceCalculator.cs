using System;
using System.Collections.Generic;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.GeoUtils {
	/// <summary>
	/// Represents an object that can calculate distance between 2 points
	/// </summary>
	public interface IDistanceCalculator {
		/// <summary>
		/// Calculates distance between 2 points on geoid surface (ignores points elevations)
		/// </summary>
		/// <param name="pt1">First point</param>
		/// <param name="pt2">Second point</param>
		/// <returns>distance between given points in meters</returns>
		double Calculate2D(IPointGeo pt1, IPointGeo pt2);
	}
}
