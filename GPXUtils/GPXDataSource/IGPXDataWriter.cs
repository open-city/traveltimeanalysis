using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GPXUtils;

namespace LK.GPXUtils.GPXDataSource {
	public interface IGPXDataWriter {
		/// <summary>
		/// Writes specific GPXPoint to the storage
		/// </summary>
		/// <param name="waypoint">The waypoint to be written</param>
		void WriteWaypoint(GPXPoint waypoint);

		/// <summary>
		/// Writes specific GPXRoute to the storage
		/// </summary>
		/// <param name="route">The route to be written</param>
		void WriteRoute(GPXRoute route);

		/// <summary>
		/// Writes specific GPXTrack to the storage
		/// </summary>
		/// <param name="track">The track to be written</param>
		void WriteTrack(GPXTrack track);
	}
}
