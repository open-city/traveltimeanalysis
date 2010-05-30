using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils;
using LK.GeoUtils.Geometry;
using LK.GPXUtils;

namespace LK.GPXUtils.Filters {
	/// <summary>
	/// Implements filter that can find and remove stops from GPXTrackSegment
	/// </summary>
	public class StopsDetector {
		/// <summary>
		/// Detects a remove stops from the specific GPXTrackSegment
		/// </summary>
		/// <param name="track">The track segment to be filtered</param>
		/// <returns>the track segment with removed stops</returns>
		public GPXTrackSegment FilterStops(GPXTrackSegment track) {
			GPXTrackSegment result = new GPXTrackSegment();
			List<GPXPoint> group = new List<GPXPoint>();

			int i = 0;
			while (i < track.NodesCount - 1) {
				GPXSegment s = new GPXSegment(track.Nodes[i], track.Nodes[i + 1]);

				if (s.AverageSpeed > 2) {
					result.Nodes.Add(track.Nodes[i]);
				}
				else {
					while (i < track.NodesCount - 1) {
						group.Add(track.Nodes[i]);
						PointGeo averagePosition = Calculations.Averaging(group);
						var maxDistance = group.Max(p => Calculations.GetDistance2D(p, averagePosition));
						GPXSegment groupsegment = new GPXSegment(group[0], group[group.Count - 1]);

						if (maxDistance > 20 || groupsegment.AverageSpeed > 10.0) {
							result.Nodes.Add(new GPXPoint(averagePosition.Latitude, averagePosition.Longitude, group[0].Time));
							result.Nodes.Add(new GPXPoint(averagePosition.Latitude, averagePosition.Longitude, group[group.Count - 1].Time));
							group.Clear();
							break;
						}
						i++;
					}
				}

				i++;
			}

			if (group.Count > 0) {
				PointGeo averagePosition = Calculations.Averaging(group);
				result.Nodes.Add(new GPXPoint(averagePosition.Latitude, averagePosition.Longitude, group[0].Time));
				result.Nodes.Add(new GPXPoint(averagePosition.Latitude, averagePosition.Longitude, group[group.Count - 1].Time));
			}

			return result;
		}
	}
}
