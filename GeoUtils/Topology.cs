using System;
using System.Collections.Generic;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.GeoUtils {
	public static class Topology {
		/// <summary>
		/// Projects the given point to the specific Line Segment
		/// </summary>
		/// <param name="toProject">The point to projecte</param>
		/// <param name="projectTo">The line, point will be projected to</param>
		/// <returns>orthogonaly projected point that lies on the specific line segment</returns>
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

			static double ToRadians(double angle) {
				return angle * Math.PI / 180;
			}
		/// <summary>
		/// Tests whether two BBoxes have non-empty intersection 
		/// </summary>
		/// <param name="bbox1">First BBox</param>
		/// <param name="bbox2">Second BBox</param>
		/// <returns>true if bbox have non-empty intersection, otherwise returns false</returns>
		public static bool Intersects(BBox bbox1, BBox bbox2) {
			foreach (PointGeo corner in bbox1.Corners) {
				if (bbox2.IsInside(corner))
					return true;
			}

			foreach (PointGeo corner in bbox2.Corners) {
				if (bbox1.IsInside(corner))
					return true;
			}

			return false;
		}

		public static PointGeo ProjectPoint(IPointGeo point, IPolyline<IPointGeo> line) {
			double minDiatance = double.PositiveInfinity;
			PointGeo closestPoint = new PointGeo();

			for (int i = 0; i < line.Nodes.Count -1; i++) {
				PointGeo projected = ProjectPoint(point, new Segment<IPointGeo>(line.Nodes[i], line.Nodes[i + 1]));
				double distance = Calculations.GetDistance2D(point, projected);
				if (distance < minDiatance) {
					minDiatance = distance;
					closestPoint = projected;
				}
			}

			return closestPoint;
		}
	}
}
