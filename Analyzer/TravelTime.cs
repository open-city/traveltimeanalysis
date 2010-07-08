using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GPXUtils;
using LK.OSMUtils.OSMDatabase;

namespace LK.Analyzer {
	/// <summary>
	/// Represents travel time between two points
	/// </summary>
	public class TravelTime {
		private SegmentInfo _segment;
		/// <summary>
		/// Gets the segment that is associated with this TravelTime object
		/// </summary>
		public SegmentInfo Segment {
			get { return _segment; }
		}

		/// <summary>
		/// Gets total travel time between Start and End point
		/// </summary>
		public TimeSpan TotalTravelTime {
			get {
				return TimeEnd - TimeStart;
			}
		}

		private DateTime _timeStart;
		/// <summary>
		/// Gets DateTime at the Start point
		/// </summary>
		public DateTime TimeStart {
			get {
				return _timeStart;
			}
		}

		private DateTime _timeEnd;
		/// <summary>
		/// Gets DateTime at the End point
		/// </summary>
		public DateTime TimeEnd {
			get {
				return _timeEnd;
			}
		}

		private List<GPXPoint> _points;
		/// <summary>
		/// Gets the list of points (with the time) on the associate segment
		/// </summary>
		public IList<GPXPoint> Points {
			get {
				return _points;
			}
		}

		/// <summary>
		/// Creates a new instance of the TravelTime object
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="points"></param>
		public TravelTime(SegmentInfo segment, DateTime start, DateTime end, IList<GPXPoint> points) {
			_segment = segment;
			_timeStart = start;
			_timeEnd = end;

			_points = new List<GPXPoint>();
			_points.AddRange(points);
		}

		public static IEnumerable<TravelTime> FromMatchedTrack(OSMDB track) {
			List<TravelTime> result = new List<TravelTime>();
			var orderedWays = track.Ways.OrderBy(way => int.Parse(way.Tags["order"].Value)).ToList();

			DateTime segmentStartTime = DateTime.MinValue;
			if(track.Nodes[orderedWays[0].Nodes[0]].Tags.ContainsTag("time"))
				segmentStartTime = DateTime.Parse(track.Nodes[orderedWays[0].Nodes[0]].Tags["time"].Value);
			
			int index = 0;
			while (track.Nodes[orderedWays[index].Nodes[0]].Tags.ContainsTag("crossroad") == false)
				index++;

			while (index < orderedWays.Count) {
				int startNodeId = int.Parse(track.Nodes[orderedWays[index].Nodes[0]].Tags["node-id"].Value);

				while (index < orderedWays.Count && track.Nodes[orderedWays[index].Nodes.Last()].Tags.ContainsTag("crossroad") == false)
					index++;

				if (index < orderedWays.Count) {
					int endNodeId = int.Parse(track.Nodes[orderedWays[index].Nodes.Last()].Tags["node-id"].Value);
					int wayId = int.Parse(orderedWays[index].Tags["way-id"].Value);
					SegmentInfo segment = new SegmentInfo() {NodeFromID = startNodeId, NodeToID = endNodeId, WayID = wayId};

					result.Add(new TravelTime(segment, DateTime.MinValue, DateTime.MinValue, new GPXPoint[] { }));
					index++;
				}
			}

			return result;
		}
	}
}
