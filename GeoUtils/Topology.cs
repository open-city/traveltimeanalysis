using System;
using System.Collections.Generic;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.GeoUtils {
	public static class Topology {
		/// <summary>
		/// Projects the point to the specific line segment
		/// </summary>
		/// <param name="toProject">The point to be projected</param>
		/// <param name="projectTo">The segment, point will be projected to</param>
		/// <returns>the orthogonaly projected point that lies on the specific line segment</returns>
		public static PointGeo ProjectPoint(IPointGeo toProject, Segment<IPointGeo> projectTo) {
			double u = ((projectTo.EndPoint.Longitude - projectTo.StartPoint.Longitude) * (toProject.Longitude - projectTo.StartPoint.Longitude) +
									(projectTo.EndPoint.Latitude - projectTo.StartPoint.Latitude) * (toProject.Latitude - projectTo.StartPoint.Latitude)) /
									(Math.Pow(projectTo.EndPoint.Longitude - projectTo.StartPoint.Longitude, 2) + Math.Pow(projectTo.EndPoint.Latitude - projectTo.StartPoint.Latitude, 2));

			u = Math.Max(u, 0);
			u = Math.Min(u, 1);

			double lon = projectTo.StartPoint.Longitude + u * (projectTo.EndPoint.Longitude - projectTo.StartPoint.Longitude);
			double lat = projectTo.StartPoint.Latitude + u * (projectTo.EndPoint.Latitude - projectTo.StartPoint.Latitude);

			return new PointGeo(lat, lon);
		}

		/// <summary>
		/// Projects the point to the specific Polyline
		/// </summary>
		/// <param name="point">The point to be projected</param>
		/// <param name="line">The polyline, point will be projected to</param>
		/// <returns>the orthogonaly projected point that lies on the specific polyline</returns>
		public static PointGeo ProjectPoint(IPointGeo point, IPolyline<IPointGeo> line) {
			double minDiatance = double.PositiveInfinity;
			PointGeo closestPoint = new PointGeo();

			for (int i = 0; i < line.Nodes.Count - 1; i++) {
				PointGeo projected = ProjectPoint(point, new Segment<IPointGeo>(line.Nodes[i], line.Nodes[i + 1]));
				double distance = Calculations.GetDistance2D(point, projected);
				if (distance < minDiatance) {
					minDiatance = distance;
					closestPoint = projected;
				}
			}

			return closestPoint;
		}

		/// <summary>
		/// Translates point in the specific direction by specific distance
		/// </summary>
		/// <param name="point">The point to be translated</param>
		/// <param name="bearing">Bearing from the original point</param>
		/// <param name="distance">Distance from the original point</param>
		/// <returns>the translated point</returns>
		public static PointGeo ProjectPoint(IPointGeo point, double bearing, double distance) {
			double lat = Math.Asin(Math.Sin(Calculations.ToRadians(point.Latitude))*Math.Cos(distance / Calculations.EarthRadius) + 
				                     Math.Cos(Calculations.ToRadians(point.Latitude))*Math.Sin(distance / Calculations.EarthRadius) * Math.Cos(Calculations.ToRadians(bearing)));

			double lon = Calculations.ToRadians(point.Longitude) + 
				           Math.Atan2(Math.Sin(Calculations.ToRadians(bearing)) * Math.Sin(distance/Calculations.EarthRadius) * Math.Cos(Calculations.ToRadians(point.Latitude)), 
                              Math.Cos(distance / Calculations.EarthRadius) - Math.Sin(Calculations.ToRadians(point.Latitude)) * Math.Sin(lat));

			return new PointGeo(Calculations.ToDegrees(lat), Calculations.ToDegrees(lon));
		}

		/// <summary>
		/// Tests whether two BBoxes have non-empty intersection 
		/// </summary>
		/// <param name="bbox1">First BBox</param>
		/// <param name="bbox2">Second BBox</param>
		/// <returns>true if bbox have non-empty intersection, otherwise returns false</returns>
		public static bool Intersects(BBox bbox1, BBox bbox2) {
			return !(bbox2.West > bbox1.East || bbox2.East < bbox1.West || bbox2.South > bbox1.North || bbox2.North < bbox1.South);
		}


	}
}
