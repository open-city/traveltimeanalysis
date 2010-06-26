using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LK.GeoUtils;
using LK.OSMUtils.OSMDatabase;
using LK.GeoUtils.Geometry;

namespace LK.MatchGPX2OSM {
	public class PathReconstructer {
		OSMDB _db;
		int _dbCounter;
		AstarPathfinder _pathfinder;

		/// <summary>
		/// Creates a new instance of the PathReconstructer
		/// </summary>
		/// <param name="graph">The RoadGraph object with the road network that will be used in the reconstruction process</param>
		public PathReconstructer(RoadGraph graph) {
			_pathfinder = new AstarPathfinder(graph);
		}

		/// <summary>
		/// Recontructs path from the candidates points
		/// </summary>
		/// <param name="matched">The list of CandidatePoints to reconstruct the path from</param>
		/// <returns>OSMDB object with the reconstructed path</returns>
		public OSMDB Reconstruct(IList<CandidatePoint> matched, bool filter) {
			_db = new OSMDB();
			_dbCounter = -1;

			if (matched.Count == 0)
				return _db;

			OSMNode node = AddNode(matched[0].MapPoint, matched[0].Layer.TrackPoint.Time);
			bool skipped = false;

			for (int i = 0; i < matched.Count - 1; i++) {
				ConnectionGeometry wayGeometry = null;
				if (Calculations.GetDistance2D(matched[i].MapPoint, matched[i + 1].MapPoint) < Calculations.EpsLength) {
					skipped = true;
					continue;
				}
				else {
					if (skipped) {
						OSMWay way = AddWay(matched[i].Road.WayID);
						way.Nodes.Add(node.ID);
						node = AddNode(matched[i].MapPoint, matched[i].Layer.TrackPoint.Time);
						way.Nodes.Add(node.ID);

						skipped = false;
					}
				}
				
				
				if (Calculations.GetDistance2D(matched[i + 1].MapPoint, matched[i].Road) < Calculations.EpsLength)
					wayGeometry = matched[i].Road;
				else if (Calculations.GetDistance2D(matched[i].MapPoint, matched[i + 1].Road) < Calculations.EpsLength)
					wayGeometry = matched[i + 1].Road;

				// both points are on the same road segment
				if (wayGeometry != null) {
					//Create a new way and add either the first  matched point or the end of the last way
					OSMWay way = AddWay(wayGeometry.WayID);
					way.Nodes.Add(node.ID);

					var points = Topology.GetNodesBetweenPoints(matched[i].MapPoint, matched[i + 1].MapPoint, wayGeometry);
					AddNodes(points, way);

					node = AddNode(matched[i + 1].MapPoint, matched[i + 1].Layer.TrackPoint.Time);
					way.Nodes.Add(node.ID);
				}
				else {
					double lenght = double.PositiveInfinity;

					// find path between matched[i] and matched[i+1]
					var pathSegments = _pathfinder.FindPath(matched[i], matched[i + 1], ref lenght);
					OSMWay way = null;

					for (int j = 0; j < pathSegments.Count; j++) {
						way = AddWay(pathSegments[j].Road.WayID);
						way.Nodes.Add(node.ID);

						var points = Topology.GetNodesBetweenPoints(pathSegments[j].From.MapPoint, pathSegments[j].To.MapPoint, pathSegments[j].Road).ToList();
						AddNodes(points, way);

						if (j < pathSegments.Count - 1) {
							node = AddNode(pathSegments[j].To.MapPoint);
							way.Nodes.Add(node.ID);
						}
					}

					node = AddNode(matched[i + 1].MapPoint, matched[i + 1].Layer.TrackPoint.Time);
					way.Nodes.Add(node.ID);
				}
			}

			return _db;
		}

		static bool IsUTurn(List<SegmentOSM> segments, Segment<IPointGeo> toTest) {
			if (Calculations.GetLength(toTest) < Calculations.EpsLength)
				return false;

			for (int i = segments.Count-1; i >= 0; i--) {
				if (Calculations.GetLength(segments[i]) < Calculations.EpsLength)
					continue;

				double firstBearing = Calculations.GetBearing(segments[i]);
				double secondBearing = Calculations.GetBearing(toTest);

				return Math.Abs(Math.Abs(firstBearing - secondBearing) - 180) < 0.01;
			}

			return false;
			//if (segments != null && toTest != null) {
			//  if (Calculations.GetDistance2D(segments.StartPoint, toTest.EndPoint) < Calculations.EpsLength)
			//    return true;

			//  double firstBearing = Calculations.GetBearing(segments);
			//  double secondBearing = Calculations.GetBearing(toTest);

			//  return Math.Abs(Math.Abs(firstBearing - secondBearing) - 180) < 0.01;
			//}

			//return false;
		}

		static int IsClose(Segment<IPointGeo> toCompare, List<SegmentOSM> segments) {
			for (int i = segments.Count -1; i >= 0; i--) {
				if (Calculations.GetDistance2D(toCompare.EndPoint, segments[i]) < Calculations.EpsLength ||
					  Calculations.GetDistance2D(segments[0].StartPoint, toCompare) < Calculations.EpsLength)
					return i;
			}

			return -1;
		}

		static void RemoveSegment(SegmentOSM toRemove, OSMDB db) {
			if (toRemove.Way.ID == -2) {
				int a = 1;
			}
			OSMNode start = (OSMNode)toRemove.StartPoint;
			OSMNode end = (OSMNode)toRemove.EndPoint;

			if (toRemove.Way.Nodes.Count == 2) {
				db.Ways.Remove(toRemove.Way);
			}
			else {
				toRemove.Way.Nodes.Remove(start.ID);
			}
		}

		class SegmentOSM : Segment<IPointGeo> {
			public OSMWay Way { get; set; }

			public SegmentOSM(IPointGeo start, IPointGeo end, OSMWay way)
				: base(start, end) {
					Way = way;
			}
		}

		public static void HFFilter(OSMDB toFilter) {
			double MaxUTurnLength = 100;

			List<SegmentOSM> all = new List<SegmentOSM>();

			foreach (var way in toFilter.Ways.OrderBy(w => int.Parse(w.Tags["order"].Value))) {
				for (int i = 0; i < way.Nodes.Count - 1; i++) {
					all.Add(new SegmentOSM(toFilter.Nodes[way.Nodes[i]], toFilter.Nodes[way.Nodes[i + 1]], way));
				}
			}

			List<SegmentOSM> open = new List<SegmentOSM>();
			IPointGeo lastValid = all[0].StartPoint;
			OSMWay lastValidWay = toFilter.Ways.Where(w => w.Nodes.Count > 0 && w.Nodes[0] == ((OSMNode)lastValid).ID).Single();

			while (all.Count > 0) {
				var segment = all[0];
				all.RemoveAt(0);
				if (segment.Way.ID == -1197) {
					int a = 1;
				}
				if(IsUTurn(open, segment)) {
					List<SegmentOSM> toRemove = new List<SegmentOSM>();
					int lastIndex = -1;
					int openIndexMatched = int.MaxValue;

					while ((lastIndex = IsClose(segment, open)) > -1) {
						if (lastIndex <= openIndexMatched) {
							openIndexMatched = lastIndex;
						}

						if (toRemove.Sum(seg => seg.Length) + segment.Length > MaxUTurnLength) {
							toRemove.Clear();
						}
						else {
							toRemove.Add(segment);
							segment = all[0];
							all.RemoveAt(0);
						}

						if (IsUTurn(toRemove, segment)) {
							break;
						}
					}

					for (int i = open.Count-1; i >= openIndexMatched; i--) {
						if (open[i].StartPoint != lastValid/* && i > openIndexMatched*/) {
							RemoveSegment(open[i], toFilter);

							toFilter.Nodes.Remove(toFilter.Nodes[((OSMNode)open[i].StartPoint).ID]);
							open.RemoveAt(i);
						}
					}

					foreach (var seg in toRemove) {
						RemoveSegment(seg, toFilter);
						toFilter.Nodes.Remove(toFilter.Nodes[((OSMNode)seg.StartPoint).ID]);
					}

					OSMNode start = (OSMNode)segment.StartPoint;
					OSMNode end = (OSMNode)segment.EndPoint;

					if (open.Count > 0) {
						open.Last().Way.Nodes[open.Last().Way.Nodes.Count - 1] = start.ID;
						SegmentOSM temp = open.Last();
						open.Remove(temp);
						open.Add(new SegmentOSM(temp.StartPoint, start, temp.Way));
						//segment.Way.Nodes[0] = open.Last().Way.Nodes.Last();
						//segment = new SegmentOSM(toFilter.Nodes[open.Last().Way.Nodes.Last()], end, segment.Way);
						//open[open.Count-1].Way.Nodes.Insert(0, ((OSMNode)lastValid).ID);
					}
					else {
						segment.Way.Nodes.Insert(0, ((OSMNode)lastValid).ID);
						lastValidWay.Nodes[lastValidWay.Nodes.Count - 1] = start.ID;
					}

					toRemove.Clear();
					open.Add(segment);

					while (open.Count > 0 && open.Sum(seg => seg.Length) > MaxUTurnLength) {
						lastValid = open[0].EndPoint;
						lastValidWay = open[0].Way;
						open.RemoveAt(0);
					}
				}
				else {
					open.Add(segment);

					while(open.Count > 0 && open.Sum(seg => seg.Length) > MaxUTurnLength) {
						lastValid = open[0].EndPoint;
						lastValidWay = open[0].Way;
						open.RemoveAt(0);
					}
				}
			}
		}

		/// <summary>
		/// Adds a new node to the OSMDB
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		OSMNode AddNode(IPointGeo point) {
			OSMNode result = new OSMNode(_dbCounter--, point.Latitude, point.Longitude);
			if (point is OSMNode) {
				result.Tags.Add(new OSMTag("node-id", ((OSMNode)point).ID.ToString()));
			}

			_db.Nodes.Add(result);

			return result;
		}

		/// <summary>
		/// Adds a new node to the OSMDB
		/// </summary>
		/// <param name="node"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		OSMNode AddNode(IPointGeo node, DateTime time) {
			OSMNode result = AddNode(node);
			result.Tags.Add(new OSMTag("time", time.ToString()));

			return result;
		}

		/// <summary>
		/// Adds nodes to the OSMDB and into OSMWay
		/// </summary>
		/// <param name="points"></param>
		/// <param name="way"></param>
		void AddNodes(IEnumerable<IPointGeo> points, OSMWay way) {
			foreach (var point in points) {
				OSMNode node = AddNode(point);
				way.Nodes.Add(node.ID);
			}
		}

		/// <summary>
		/// Adds a new way to the OSMDB
		/// </summary>
		/// <param name="wayID"></param>
		/// <returns></returns>
		OSMWay AddWay(int wayID) {
			OSMWay result = new OSMWay(_dbCounter--);
			result.Tags.Add(new OSMTag("way-id", wayID.ToString()));
			result.Tags.Add(new OSMTag("order", (_db.Ways.Count + 1).ToString()));

			_db.Ways.Add(result);

			return result;
		}
	}
}
