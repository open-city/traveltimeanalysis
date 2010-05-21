using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GPXUtils;

namespace LK.GPXUtils.GPXDataSource {
	public delegate void GPXTrackReadHandler(GPXTrack track);
	public delegate void GPXRouteReadHandler(GPXRoute route);
	public delegate void GPXWaypointReadHandler(GPXPoint waypoint);

	public interface IGPXDataReader {
		/// <summary>
		/// Occurs when a track is read from the datasource
		/// </summary>
		event GPXTrackReadHandler TrackRead;

		/// <summary>
		/// Occurs when a route is read from the datasource
		/// </summary>
		event GPXRouteReadHandler RouteRead;

		/// <summary>
		/// Occurs when a waypoint is read from the datasource
		/// </summary>
		event GPXWaypointReadHandler WaypointRead;
	}
}
