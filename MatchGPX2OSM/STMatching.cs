using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils;
using LK.GeoUtils.Geometry;
using LK.GPXUtils;
using LK.OSMUtils.OSMDatabase;

namespace LK.MatchGPX2OSM {
	public class STMatching {
		public static int MaxCandidatesCount = 5;

		RoadGraph _graph;
		CandidatesGraph _candidatesGraph;

		public STMatching(RoadGraph graph) {
			_graph = graph;
		}

		/// <summary>
		/// Recontruct path from the candidates points
		/// </summary>
		/// <param name="matched"></param>
		/// <returns></returns>
		public OSMDB Reconstruct(IList<CandidatePoint> matched) {
			Astar pathfinder = new Astar(_graph);			
			OSMDB result = new OSMDB();
			int counter = -1;

			OSMNode node = AddNodeToPath(result, ref counter, matched[0]);// new OSMNode(counter--, matched[0].Latitude, matched[0].Longitude);
			node.Tags.Add(new OSMTag("time", matched[0].Layer.TrackPoint.Time.ToString()));

			for (int i = 0; i < matched.Count - 1; i++) {
				OSMWay way = new OSMWay(counter--);
				result.Ways.Add(way);
				way.Nodes.Add(node.ID);

				ConnectionGeometry wayGeometry = null;
				if (Calculations.GetDistance2D(matched[i + 1], matched[i].Road) < Calculations.EpsLength)
					wayGeometry = matched[i].Road;
				else if (Calculations.GetDistance2D(matched[i], matched[i + 1].Road) < Calculations.EpsLength)
					wayGeometry = matched[i + 1].Road;

				// both points are on the same way
				if (wayGeometry != null) {
					way.Tags.Add(new OSMTag("way-id", wayGeometry.WayID.ToString()));

					var points = GetNodesBetweenPoints(matched[i], matched[i + 1], wayGeometry);
					foreach (var point in points) {
						node = AddNodeToPath(result, ref counter, point);
						way.Nodes.Add(node.ID);
					}
				}
				else {
					double lenght = 0;
					// find path between matched[i] and matched[i+1]
					var paths = pathfinder.FindPath(matched[i], matched[i + 1], ref lenght).ToList();

					int lastWayId = 0;
					if (paths.Count > 0) {
						lastWayId = paths[0].Road.WayID;
						way.Tags.Add(new OSMTag("way-id", paths[0].Road.WayID.ToString()));
					}

					for (int j = 0; j < paths.Count; j++) {
						if (j > 0) {
							node = AddNodeToPath(result, ref counter, paths[j].From.Position);
							way.Nodes.Add(node.ID);
						}

						// Split way if original way ids differs
						if (paths[j].Road.WayID != lastWayId) {
							lastWayId = paths[j].Road.WayID;
							way = new OSMWay(counter--);
							result.Ways.Add(way);
							way.Nodes.Add(node.ID);
							way.Tags.Add(new OSMTag("way-id", paths[j].Road.WayID.ToString()));
						}
						
						var points = GetNodesBetweenPoints(paths[j].From.Position, paths[j].To.Position, paths[j].Road).ToList();
						foreach (var point in points) {
							node = AddNodeToPath(result, ref counter, point);
							way.Nodes.Add(node.ID);
						}
					}
				}

				node = AddNodeToPath(result, ref counter, matched[i + 1]);
				node.Tags.Add(new OSMTag("time", matched[i + 1].Layer.TrackPoint.Time.ToString()));
				way.Nodes.Add(node.ID);
			}

			return result;
		}

		/// <summary>
		/// Adds a new node to the OSMDB
		/// </summary>
		/// <param name="db"></param>
		/// <param name="counter"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		OSMNode AddNodeToPath(OSMDB db, ref int counter, IPointGeo node) {
			OSMNode result = new OSMNode(counter--, node.Latitude, node.Longitude);
			db.Nodes.Add(result);

			return result;
		}
		
		/// <summary>
		/// Matches the given GPX track to the map
		/// </summary>
		/// <param name="gpx"></param>
		/// <returns>OSM db that represents matched track</returns>
		public OSMDB Match(GPXTrackSegment gpx) {
			IList<CandidatePoint> matched = FindMatchedCandidates(gpx).ToList();
			return Reconstruct(matched);
		}

		/// <summary>
		/// Returns nodes on the specific path between two points
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		IEnumerable<IPointGeo> GetNodesBetweenPoints(IPointGeo from, IPointGeo to, IPolyline<IPointGeo> path) {
			var segments = path.Segments;

			List<IPointGeo> result = new List<IPointGeo>();

			int fromIndex = -1;
			int toIndex = -1;
			for (int i = 0; i < segments.Count; i++) {
				if (Calculations.GetDistance2D(from, segments[i]) < Calculations.EpsLength) {
					if (fromIndex > -1 && toIndex > -1 && toIndex < fromIndex)
						;
					else
						fromIndex = i;
				}
				if (Calculations.GetDistance2D(to, segments[i]) < Calculations.EpsLength) {
					if (fromIndex > -1 && toIndex > -1 && toIndex > fromIndex)
						;
					else
						toIndex = i;
				}
			}
			if (fromIndex == -1 || toIndex == -1)
				return result;

			if (fromIndex == toIndex - 1) {
				result.Add(segments[fromIndex].EndPoint);
			}
			else if (fromIndex - 1 == toIndex) {
				result.Add(segments[toIndex].EndPoint);
			}
			else if (fromIndex < toIndex) {
				for (int i = fromIndex; i < toIndex; i++) {
					result.Add(segments[i].EndPoint);
				}
			}
			else if (toIndex < fromIndex) {
				for (int i = fromIndex; i > toIndex; i--) {
					result.Add(segments[i].StartPoint);
				}
			}

			return result;
		}
		/// <summary>
		/// Finds the best matching sequence of candidate points
		/// </summary>
		/// <param name="gpx">The GPS track</param>
		/// <returns></returns>
		IEnumerable<CandidatePoint> FindMatchedCandidates(GPXTrackSegment gpx) {
			_candidatesGraph = new CandidatesGraph();

			//Find candidate points + ObservationProbability
			foreach (var gpxPoint in gpx.Nodes) {
				var candidates = FindCandidatePoints(gpxPoint);

				_candidatesGraph.Layers.Add(CreateLayer(gpxPoint, candidates));
			}

			_candidatesGraph.ConnectLayers();
			AssignTransmissionProbability();

			// Find matched sequence
			foreach (var candidate in _candidatesGraph.Layers[0].Candidates) {
				candidate.HighestProbability = candidate.ObservationProbability;
			}
			for (int i = 0; i < _candidatesGraph.Layers.Count - 1; i++) {
				foreach (var candidate in _candidatesGraph.Layers[i + 1].Candidates) {
					foreach (var connection in candidate.IncomingConnections) {
						double score = connection.From.HighestProbability + candidate.ObservationProbability * connection.TransmissionProbability;
						if (score > candidate.HighestProbability) {
							candidate.HighestProbability = score;
							candidate.HighesScoreParent = connection.From;
						}
					}
				}
			}

			List<CandidatePoint> result = new List<CandidatePoint>();
			CandidatePoint current = new CandidatePoint() { HighestProbability = double.NegativeInfinity };
			foreach (var point in _candidatesGraph.Layers[_candidatesGraph.Layers.Count - 1].Candidates) {
				if (point.HighestProbability > current.HighestProbability) {
					current = point;
				}
			}

			while (current != null) {
				result.Add(current);
				current = current.HighesScoreParent;
			}

			result.Reverse();
			return result;
		}

		/// <summary>
		/// Finds all candidates points for given GPS track point
		/// </summary>
		/// <param name="gpxPt">GPS point</param>
		/// <returns>Collection of points candidate points on road segments</returns>
		public IEnumerable<CandidatePoint> FindCandidatePoints(GPXPoint gpxPt) {
			List<CandidatePoint> result = new List<CandidatePoint>();
			BBox gpxBbox = new BBox(new IPointGeo[] { gpxPt });
			gpxBbox.Inflate(0.0007, 0.0011);

			foreach (var road in _graph.ConnectionGeometries) {
				if (Topology.Intersects(gpxBbox, road.BBox)) {
					IPointGeo projectedPoint = Topology.ProjectPoint(gpxPt, road);
					result.Add(new CandidatePoint() { Latitude = projectedPoint.Latitude, Longitude = projectedPoint.Longitude, Road = road, ObservationProbability = CalculateObservationProbability(gpxPt, projectedPoint) });
				}
			}

			return result;
		}

		/// <summary>
		/// Creates a new layer
		/// </summary>
		/// <param name="originalPoint">GPX track point</param>
		/// <param name="candidates">Candidate points for the original point</param>
		/// <returns></returns>
		CandidateGraphLayer CreateLayer(GPXPoint originalPoint, IEnumerable<CandidatePoint> candidates) {
			CandidateGraphLayer result = new CandidateGraphLayer() { TrackPoint = originalPoint };
			result.Candidates.AddRange(candidates.OrderByDescending(c => c.ObservationProbability).Take(Math.Min(candidates.Count(), STMatching.MaxCandidatesCount)));

			foreach (var candidate in result.Candidates) {
				candidate.Layer = result;
			}

			return result;
		}

		/// <summary>
		/// Calculates observation probability
		/// </summary>
		/// <param name="original">GPS track point</param>
		/// <param name="candidate">Candidate point</param>
		/// <returns>double representing probability that GPS track point corresponds with Candidate point</returns>
		double CalculateObservationProbability(GPXPoint original, IPointGeo candidate) {
			double sigma = 30;
			double distance = Calculations.GetDistance2D(original, candidate);
			return Math.Exp(-distance * distance / (2 * sigma * sigma)) / (sigma * Math.Sqrt(Math.PI * 2));
		}

		/// <summary>
		/// Assigns transmission probability to every connection in the graph
		/// </summary>
		void AssignTransmissionProbability() {
			foreach (var layer in _candidatesGraph.Layers) {
				foreach (var candidatePoint in layer.Candidates) {
					foreach (var connection in candidatePoint.OutgoingConnections) {
						connection.TransmissionProbability = CalculateTransmissionProbability(connection);
					}
				}
			}
		}
		/// <summary>
		/// Calculates transmission probability for connection
		/// </summary>
		/// <param name="c">Connection</param>
		/// <returns>double value representing transmission probability</returns>
		double CalculateTransmissionProbability(CandidatesConection c) {
			double gcd = Calculations.GetDistance2D(c.From, c.To);
			double shortestPath = FindShortestPath(c.From, c.To);

			if (gcd == 0 && shortestPath == 0)
				return 1;
			else
				return gcd / shortestPath;
		}

		/// <summary>
		/// Finds shortest path between two points along routes
		/// </summary>
		/// <param name="from">Start point</param>
		/// <param name="to">Destination point</param>
		/// <returns>length of the path in meters</returns>
		double FindShortestPath(CandidatePoint from, CandidatePoint to) {
			if (from.Road == to.Road) {
				return Calculations.GetPathLength(from, to, from.Road);
			}
			else {
				Astar pathfinder = new Astar(_graph);
				double length = double.PositiveInfinity;
				pathfinder.FindPath(from, to, ref length);
				return length;
			}
		}
	}
}
