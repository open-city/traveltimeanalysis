using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GPXUtils;

namespace LK.GPXUtils.Filters {
	/// <summary>
	/// Implements filter that reduces frequncy of the GPS track
	/// </summary>
	public class FrequencyFilter {
		public GPXTrackSegment Filter(TimeSpan minInterval, GPXTrackSegment track) {
			GPXTrackSegment result = new GPXTrackSegment();

			if (track.Nodes.Count == 0)
				return result;

			GPXPoint last = track.Nodes[0];
			result.Nodes.Add(last);

			int index = 1;
			while (index < track.Nodes.Count) {
				if (track.Nodes[index].Time - last.Time >= minInterval) {
					last = track.Nodes[index];
					result.Nodes.Add(last);
				}
				index++;
			}
			return result;
		}
	}
}
