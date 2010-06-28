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

		Polyline<IPointGeo> CreateLine(IPointGeo from, IPointGeo to, ConnectionGeometry road) {
			Polyline<IPointGeo> line = new Polyline<IPointGeo>();
			line.Nodes.Add(from);

			var points = Topology.GetNodesBetweenPoints(from, to, road);
			foreach (var point in points) {
				line.Nodes.Add(point);
			}

			line.Nodes.Add(to);
			return line;
		}

		public List<Polyline<IPointGeo>> Reconstruct(IList<CandidatePoint> matched) {
			List<Polyline<IPointGeo>> result = new List<Polyline<IPointGeo>>();

			for (int i = 0; i < matched.Count - 1; i++) {
				ConnectionGeometry wayGeometry = null;
				if (Calculations.GetDistance2D(matched[i + 1].MapPoint, matched[i].Road) < Calculations.EpsLength)
					wayGeometry = matched[i].Road;
				else if (Calculations.GetDistance2D(matched[i].MapPoint, matched[i + 1].Road) < Calculations.EpsLength)
					wayGeometry = matched[i + 1].Road;

				// both points are on the same road segment
				if (wayGeometry != null) {
					result.Add(CreateLine(matched[i].MapPoint, matched[i + 1].MapPoint, wayGeometry));
				}
				else {
					double lenght = double.PositiveInfinity;

					// find path between matched[i] and matched[i+1]
					var pathSegments = _pathfinder.FindPath(matched[i], matched[i + 1], ref lenght);
					foreach (var pathSegment in pathSegments) {
						result.Add(CreateLine(pathSegment.From.MapPoint, pathSegment.To.MapPoint, pathSegment.Road));
					}
				}
			}
			return result;
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

			if (filter) {
				FilterUTurns(_db, 100);
			}
			return _db;
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



		bool IsUTurn(Polyline<IPointGeo> previousLine, Polyline<IPointGeo> toTest) {
			if (previousLine == null || toTest == null)
				return false;

			if (Calculations.GetLength(toTest) < Calculations.EpsLength)
				return false;

			Segment<IPointGeo> previousLineSegment = null;
			for (int i = previousLine.Segments.Count -1; i >= 0; i--) {
				if (previousLine.Segments[i].Length > Calculations.EpsLength) {
					previousLineSegment = previousLine.Segments[i];
					break;
				}
			}

			Segment<IPointGeo> toTestSegment = null;
			for (int i = 0; i < toTest.Segments.Count; i++) {
				if (toTest.Segments[i].Length > Calculations.EpsLength) {
					toTestSegment = toTest.Segments[i];
					break;
				}
			}

			if (previousLineSegment != null && toTestSegment != null) {
				double firstBearing = Calculations.GetBearing(previousLineSegment);
				double secondBearing = Calculations.GetBearing(toTestSegment);

				return Math.Abs(Math.Abs(firstBearing - secondBearing) - 180) < 0.01;
			}

			return false;
		}

		int GetPreviousNonZeroLengthLineIndex(IList<Polyline<IPointGeo>> lines, int maxIndex) {
			for (int i = maxIndex; i >= 0; i--) {
				if (lines[i].Length > Calculations.EpsLength)
					return i;
			}

			return -1;
		}
		
		public void FilterUturns(IList<Polyline<IPointGeo>> path, double maxUTurnLength) {
			IPointGeo lastNonUTurn = path[0].Nodes[0];
			Polyline<IPointGeo> lastNonUTurnWay = path[0];

			int index = 0;
			while (index < path.Count) {
				Polyline<IPointGeo> current = path[index];

				if (current.Length < Calculations.EpsLength) {
					index++;
					continue;
				}
				
				if (current.Length > maxUTurnLength) {
					lastNonUTurn = current.Nodes.Last();
					lastNonUTurnWay = current;
					index++;
					continue;
				}

				int previousLineIndex = GetPreviousNonZeroLengthLineIndex(path, index - 1);

				if (previousLineIndex > -1 && IsUTurn(path[previousLineIndex], current)) {
					for (int i = previousLineIndex + 1; i < index; i++) {
						path[i].Nodes.Clear();
					}

					// forth and back to the same point
					if (Calculations.GetDistance2D(path[previousLineIndex].Nodes[0], current.Nodes[current.Nodes.Count - 1]) < Calculations.EpsLength) {
						IPointGeo from = path[previousLineIndex].Nodes[0];
						IPointGeo to = current.Nodes[current.Nodes.Count - 1];

						path[previousLineIndex].Nodes.Clear();
						current.Nodes.Clear();
						current.Nodes.Add(from);
						current.Nodes.Add(to);
					}
					else {
						int currentSegmentIndex = 0;
						int lastSegmentIndex = path[previousLineIndex].Segments.Count - 1;

						int j = 0;
						while (j < current.Segments.Count &&
									 Calculations.GetDistance2D(current.Segments[j].EndPoint, path[previousLineIndex], ref lastSegmentIndex) < Calculations.EpsLength) {
										 currentSegmentIndex = j;
										 j++;
						}

						// delete the whole previous line
						if (lastSegmentIndex == 0) {
							IPointGeo from = path[previousLineIndex].Nodes[0];
							IPointGeo to = current.Segments[currentSegmentIndex].EndPoint;

							while (current.Nodes[0] != to) {
								current.Nodes.RemoveAt(0);
							}
							current.Nodes.Insert(0, from);

							path[previousLineIndex].Nodes.Clear();
						}
						else if (currentSegmentIndex == current.Segments.Count - 1) {
							IPointGeo from = path[previousLineIndex].Segments[lastSegmentIndex].StartPoint;
							IPointGeo to = current.Nodes[current.Nodes.Count - 1];

							while (path[previousLineIndex].Nodes[path[previousLineIndex].Nodes.Count - 1] != from) {
								path[previousLineIndex].Nodes.RemoveAt(path[previousLineIndex].Nodes.Count - 1);
							}

							path[previousLineIndex].Nodes.Add(to);
							current.Nodes.Clear();
						}
					}
				}
				else {
					index++;
				}
			
			}
		}

		public OSMDB SaveToOSM(IList<Polyline<IPointGeo>> path) {
			_db = new OSMDB();
			_dbCounter = -1;

			IPointGeo lastPoint = null;
			OSMNode node = null;

			foreach (var line in path) {
				if (line.Nodes.Count == 0)
					continue;

				if (line.Nodes.Count == 1)
					throw new Exception();

				OSMWay way = new OSMWay(_dbCounter--);
				_db.Ways.Add(way);
				foreach (var point in line.Nodes) {
					if (point != lastPoint) {
						node = new OSMNode(_dbCounter--, point.Latitude, point.Longitude);
						_db.Nodes.Add(node);
					}
					way.Nodes.Add(node.ID);
				}
			}

			return _db;
		}
		
		/// <summary>
		/// Filters u-turns shorter than maxUturnLength from the database
		/// </summary>
		/// <param name="toFilter">OSMDb with the result of the Reconstruct method</param>
		/// <param name="maxUTurnLength">Maximal u-turn length in meters</param>
		public static void FilterUTurns(OSMDB toFilter, double maxUTurnLength) {
			List<SegmentOSM> all = new List<SegmentOSM>();
			//Prepare segments of all ways
			foreach (var way in toFilter.Ways.OrderBy(w => int.Parse(w.Tags["order"].Value))) {
				for (int i = 0; i < way.Nodes.Count - 1; i++) {
					all.Add(new SegmentOSM(toFilter.Nodes[way.Nodes[i]], toFilter.Nodes[way.Nodes[i + 1]], way));
				}
			}

			List<SegmentOSM> open = new List<SegmentOSM>();
			//last point that can not be uturn and thus can not be deleted
			IPointGeo lastValid = all[0].StartPoint;
			OSMWay lastValidWay = toFilter.Ways.Where(w => w.Nodes.Count > 0 && w.Nodes[0] == ((OSMNode)lastValid).ID).Single();

			while (all.Count > 0) {
				var segment = all[0];
				all.RemoveAt(0);

				// current segment goes in the opposite direction then the last one
				if (IsUTurn(open, segment)) {
					List<SegmentOSM> toRemove = new List<SegmentOSM>();
					int lastIndex = -1;
					int openIndexMatched = int.MaxValue;
					bool moveToNext = false;

					// while it follows path in the open list, i.e. u-turn continues
					while ((lastIndex = IsClose(segment, open)) > -1) {
						if (lastIndex <= openIndexMatched) {
							openIndexMatched = lastIndex;
						}

						// stop if length of the u-turn is greather then maxUTurnLength
						if (toRemove.Sum(seg => seg.Length) + segment.Length > maxUTurnLength) {
							moveToNext = true;
							AddToList(segment, open, maxUTurnLength, ref lastValid, ref lastValidWay);
							break;
						}
						else {
							if (all.Count > 0) {
								toRemove.Add(segment);

								segment = all[0];
								all.RemoveAt(0);
							}
							else
								// or if there are no segments left
								break;
						}

						// or another u-turn occures
						if (IsUTurn(toRemove, segment)) {
							break;
						}
					}

					if (moveToNext)
						continue;

					// remove u-turn
					OSMNode start = (OSMNode)segment.StartPoint;
					OSMNode end = (OSMNode)segment.EndPoint;

					// remove u-turn from the open list (outwards part)
					for (int i = open.Count - 1; i >= openIndexMatched; i--) {
						if (open[i].StartPoint != lastValid) {
							if (i == openIndexMatched && ((OSMNode)open[i].StartPoint).Tags.ContainsTag("time") == false)
								continue;

							RemoveSegment(open[i], toFilter);
							toFilter.Nodes.Remove(toFilter.Nodes[((OSMNode)open[i].StartPoint).ID]);
							open.RemoveAt(i);
						}
					}

					// remove u-turn from the toRemove list (backwards part)
					foreach (var seg in toRemove) {
						RemoveSegment(seg, toFilter);
						toFilter.Nodes.Remove(toFilter.Nodes[((OSMNode)seg.StartPoint).ID]);
					}

					// adjust connection between the part preceding u-turn and the part contiguous u-turn
					open.Last().Way.Nodes[open.Last().Way.Nodes.Count - 1] = start.ID;
					SegmentOSM temp = open.Last();
					open.Remove(temp);

					SegmentOSM modified = new SegmentOSM(temp.StartPoint, start, temp.Way);
					if (Calculations.GetLength(modified) < Calculations.EpsLength) {
						RemoveSegment(temp, toFilter);
						segment.Way.Nodes[0] = ((OSMNode)temp.StartPoint).ID;
						modified = new SegmentOSM(temp.StartPoint, end, temp.Way);
						toFilter.Nodes.Remove(start);
					}

					toRemove.Clear();
					AddToList(modified, open, maxUTurnLength, ref lastValid, ref lastValidWay);
					AddToList(segment, open, maxUTurnLength, ref lastValid, ref lastValidWay);
				}
				//it is not u-turn - add segment to open list
				else {
					AddToList(segment, open, maxUTurnLength, ref lastValid, ref lastValidWay);
				}
			}
		}

		/// <summary>
		/// Adds segment into list and delete previously add ways if length exceeded max length
		/// </summary>
		/// <param name="segment"></param>
		/// <param name="list"></param>
		/// <param name="maxLength"></param>
		/// <param name="lastPoint"></param>
		/// <param name="lastWay"></param>
		static void AddToList(SegmentOSM segment, List<SegmentOSM> list, double maxLength, ref IPointGeo lastPoint, ref OSMWay lastWay) {
			list.Add(segment);

			while (list.Count > 0 && list.Sum(seg => seg.Length) > maxLength) {
				lastPoint = list[0].EndPoint;
				lastWay = list[0].Way;
				list.RemoveAt(0);
			}
		}
		/// <summary>
		/// Tests whether toTest segment goes in the opposite direction then last non-zero length segment in the segments list
		/// </summary>
		/// <param name="segments"></param>
		/// <param name="toTest"></param>
		/// <returns></returns>
		static bool IsUTurn(List<SegmentOSM> segments, Segment<IPointGeo> toTest) {
			if (Calculations.GetLength(toTest) < Calculations.EpsLength)
				return false;

			for (int i = segments.Count - 1; i >= 0; i--) {
				if (Calculations.GetLength(segments[i]) < Calculations.EpsLength)
					continue;

				double firstBearing = Calculations.GetBearing(segments[i]);
				double secondBearing = Calculations.GetBearing(toTest);

				return Math.Abs(Math.Abs(firstBearing - secondBearing) - 180) < 0.01;
			}

			return false;
		}

		/// <summary>
		/// Tests whether toCompare segment ends close to any segment in the segments list
		/// </summary>
		/// <param name="toCompare"></param>
		/// <param name="segments"></param>
		/// <returns>the index of the segment from the segments list, or -1</returns>
		static int IsClose(Segment<IPointGeo> toCompare, List<SegmentOSM> segments) {
			for (int i = segments.Count - 1; i >= 0; i--) {
				if (Calculations.GetDistance2D(toCompare.EndPoint, segments[i]) < Calculations.EpsLength ||
						Calculations.GetDistance2D(segments[0].StartPoint, toCompare) < Calculations.EpsLength)
					return i;
			}

			return -1;
		}

		/// <summary>
		/// Removes the toRemove segment from the database
		/// </summary>
		/// <param name="toRemove"></param>
		/// <param name="db"></param>
		static void RemoveSegment(SegmentOSM toRemove, OSMDB db) {
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
	}
}
