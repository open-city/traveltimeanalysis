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
	}
}
