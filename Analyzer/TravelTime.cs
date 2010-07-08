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

		static double GetLength(OSMWay way, OSMDB db) {
			double result = 0;
			for (int i = 0; i < way.Nodes.Count -1; i++) {
				result += LK.GeoUtils.Calculations.GetDistance2D(db.Nodes[way.Nodes[i]], db.Nodes[way.Nodes[i + 1]]);
			}

			return result;
		}

		static DateTime InterpolateEndTime(OSMDB db, IList<OSMWay> ways, int segmentIndex) {
			double lenghtBefore = 0;
			int i = segmentIndex;
			while (i >= 0 && db.Nodes[ways[i].Nodes[0]].Tags.ContainsTag("time") == false) {
				i--;
				lenghtBefore += GetLength(ways[i], db);
			}

			DateTime lastTime = DateTime.MinValue;
			if (i >= 0)
				lastTime = DateTime.Parse(db.Nodes[ways[i].Nodes[0]].Tags["time"].Value);
			else
				throw new ArgumentException("Can not find segment start time");

			double lengthAfter = 0;
			i = segmentIndex;
			while (i < ways.Count && db.Nodes[ways[i].Nodes.Last()].Tags.ContainsTag("time") == false) {
				i++;
				lengthAfter += GetLength(ways[i], db);
			}

			DateTime nextTime = DateTime.MinValue;
			if (i < ways.Count)
				nextTime = DateTime.Parse(db.Nodes[ways[i].Nodes.Last()].Tags["time"].Value);
			else
				throw new ArgumentException("Can not find segment end time");

			double miliseconds = (nextTime - lastTime).TotalMilliseconds;
			double totalLength = lenghtBefore + GetLength(ways[segmentIndex], db) + lengthAfter;

			return lastTime.AddMilliseconds(miliseconds * (lenghtBefore + GetLength(ways[segmentIndex], db)) / totalLength);
		}

		static DateTime InterpolateStartTime(OSMDB db, IList<OSMWay> ways, int segmentIndex) {
			double lenghtBefore = 0;
			int i = segmentIndex;
			while (i >= 0 && db.Nodes[ways[i].Nodes[0]].Tags.ContainsTag("time") == false) {
				i--;
				lenghtBefore += GetLength(ways[i], db);				
			}

			DateTime lastTime = DateTime.MinValue;
			if (i >= 0)
				lastTime = DateTime.Parse(db.Nodes[ways[i].Nodes[0]].Tags["time"].Value);
			else
				throw new ArgumentException("Can not find segment start time");

			double lengthAfter = 0;
			i = segmentIndex;
			while (i < ways.Count && db.Nodes[ways[i].Nodes.Last()].Tags.ContainsTag("time") == false) {
				i++;
				lengthAfter += GetLength(ways[i], db);
			}

			DateTime nextTime = DateTime.MinValue;
			if (i < ways.Count)
				nextTime = DateTime.Parse(db.Nodes[ways[i].Nodes.Last()].Tags["time"].Value);
			else
				throw new ArgumentException("Can not find segment end time");

			double miliseconds = (nextTime - lastTime).TotalMilliseconds;
			double totalLength = lenghtBefore + GetLength(ways[segmentIndex], db) + lengthAfter;

			return lastTime.AddMilliseconds(miliseconds * lenghtBefore / totalLength);
		}
		
		public static IEnumerable<TravelTime> FromMatchedTrack(OSMDB track) {
			List<TravelTime> result = new List<TravelTime>();
			var orderedWays = track.Ways.OrderBy(way => int.Parse(way.Tags["order"].Value)).ToList();
			
			//Find start of the first segment
			int index = 0;
			while (track.Nodes[orderedWays[index].Nodes[0]].Tags.ContainsTag("crossroad") == false)
				index++;

			while (index < orderedWays.Count) {
				int startNodeId = int.Parse(track.Nodes[orderedWays[index].Nodes[0]].Tags["node-id"].Value);

				DateTime segmentStartTime = DateTime.MinValue;
				if (track.Nodes[orderedWays[index].Nodes[0]].Tags.ContainsTag("time"))
					segmentStartTime = DateTime.Parse(track.Nodes[orderedWays[index].Nodes[0]].Tags["time"].Value);
				else
					segmentStartTime = InterpolateStartTime(track, orderedWays, index);

				List<GPXPoint> points = new List<GPXPoint>();
				while (index < orderedWays.Count && track.Nodes[orderedWays[index].Nodes.Last()].Tags.ContainsTag("crossroad") == false) {
					if (track.Nodes[orderedWays[index].Nodes.Last()].Tags.ContainsTag("time")) {
						points.Add(new GPXPoint(track.Nodes[orderedWays[index].Nodes.Last()].Latitude, track.Nodes[orderedWays[index].Nodes.Last()].Longitude,
							DateTime.Parse(track.Nodes[orderedWays[index].Nodes.Last()].Tags["time"].Value)));
					}

					index++;
				}

				if (index < orderedWays.Count) {
					int endNodeId = int.Parse(track.Nodes[orderedWays[index].Nodes.Last()].Tags["node-id"].Value);
					
					DateTime segmentEndTime = DateTime.MinValue;
					if (track.Nodes[orderedWays[index].Nodes.Last()].Tags.ContainsTag("time"))
						segmentEndTime = DateTime.Parse(track.Nodes[orderedWays[index].Nodes.Last()].Tags["time"].Value);
					else
						segmentEndTime = InterpolateEndTime(track, orderedWays, index);

					int wayId = int.Parse(orderedWays[index].Tags["way-id"].Value);
					SegmentInfo segment = new SegmentInfo() {NodeFromID = startNodeId, NodeToID = endNodeId, WayID = wayId};

					result.Add(new TravelTime(segment, segmentStartTime, segmentEndTime, points));
					index++;
				}
			}

			return result;
		}
	}
}
