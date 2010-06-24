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
		AstarPathfinder _pathfinder;

		/// <summary>
		/// Create a new instance of the STMatching class
		/// </summary>
		/// <param name="graph">The RoadGraph object that represents road network</param>
		public STMatching(RoadGraph graph) {
			_graph = graph;
			_pathfinder = new AstarPathfinder(_graph);
		}
		
		/// <summary>
		/// Matches the given GPX track to the map
		/// </summary>
		/// <param name="gpx">The GPS track log</param>
		/// <returns>List of the CandidatePoints that match GPS log the best</returns>
		public IList<CandidatePoint> Match(GPXTrackSegment gpx) {
			_candidatesGraph = new CandidatesGraph();

			//Find candidate points + ObservationProbability
			foreach (var gpxPoint in gpx.Nodes) {
				var candidates = FindCandidatePoints(gpxPoint);

				_candidatesGraph.CreateLayer(gpxPoint, candidates.OrderByDescending(c => c.ObservationProbability).Take(Math.Min(candidates.Count(), STMatching.MaxCandidatesCount)));
			}

			// Calculate transmission probability
			_candidatesGraph.ConnectLayers();
			AssignTransmissionProbability();

			//Evaluates paths in the graph
			EvaluateGraph();

			//Extract result
			List<CandidatePoint> result = new List<CandidatePoint>();
			CandidatePoint current = _candidatesGraph.Layers[_candidatesGraph.Layers.Count - 1].Candidates.OrderByDescending(c => c.HighestProbability).FirstOrDefault();

			while (current != null) {
				result.Add(current);
				current = current.HighesScoreParent;
			}

			result.Reverse();
			return result;
		}

		/// <summary>
		/// Traverses through the CandidatesGraph and finds ancestor with the highest probability for every CandidatePoint in the graph
		/// </summary>
		void EvaluateGraph() {
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
					Segment<IPointGeo> roadSegment;
					IPointGeo projectedPoint = Topology.ProjectPoint(gpxPt, road, out roadSegment);
					if (projectedPoint.Latitude == 50.4984849 && projectedPoint.Longitude == 16.1141259) {
						int a = 1;
					}
					result.Add(new CandidatePoint() { MapPoint = projectedPoint,
						                                Road = road, RoadSegment = roadSegment,
																						ObservationProbability = CalculateObservationProbability(gpxPt, projectedPoint) });
				}
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
		double CalculateTransmissionProbability(CandidatesConnection c) {
			double gcd = Calculations.GetDistance2D(c.From.MapPoint, c.To.MapPoint);
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
				return Calculations.GetPathLength(from.MapPoint, to.MapPoint, from.Road);
			}
			else {
				double length = double.PositiveInfinity;
				_pathfinder.FindPath(from, to, ref length);
				return length;
			}
		}
	}
}
