using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils;
using LK.GeoUtils.Geometry;
using LK.GPXUtils;

namespace LK.MatchGPX2OSM {
	public class STMatching {
		protected RoadGraph _graph;
		List<CandidateGraphLayer> _layers;

		public STMatching(RoadGraph graph) {
			_graph = graph;
		}

		public IEnumerable<CandidatePoint> FindCandidatePoints(GPXPoint gpxPt) {
			List<CandidatePoint> result = new List<CandidatePoint>();

			foreach (var road in _graph.ConnectionGeometries) {
				if (road.BBox.IsInside2D(gpxPt)) {
					PointGeo projectedPoint = Topology.ProjectPoint(gpxPt, road);
					result.Add(new CandidatePoint() { Latitude = projectedPoint.Latitude, Longitude = projectedPoint.Longitude, Road = road, ObservationProbability = CalculateObservationProbability(gpxPt, projectedPoint) });
				}
			}

			return result;
		}

		public double CalculateObservationProbability(GPXPoint original, PointGeo candidate) {
			double distance = Calculations.GetDistance2D(original, candidate);
			return Math.Exp(-distance * distance / (2 * 20 * 20)) / (20 * Math.Sqrt(Math.PI * 2));
		}

		public double CalculateTransmissionProbability(CandidatesConection c) {
			double gcd = Calculations.GetDistance2D(c.From, c.To);
			double shortestPath = ComputeShortestPath(c.From, c.To);

			return gcd / shortestPath;
		}

		public double ComputeShortestPath(CandidatePoint from, CandidatePoint to) {
			if (from.Road == to.Road) {
				return ComputeDistanceOnRoad(from, to, from.Road);
			}
			return 0;
		}

		double ComputeDistanceOnRoad(CandidatePoint from, CandidatePoint to, ConnectionGeometry road) {
			double eps = 1e-4;

			double distance = 0;

			List<Segment<IPointGeo>> segments = new List<Segment<IPointGeo>>(road.GetSegments());
			int seg1 = -1;
			int seg2 = -1;

			for (int i = 0; i < segments.Count; i++) {
				if (Calculations.GetDistance2D(from, segments[i]) < eps) {
					seg1 = i;
				}
				if (Calculations.GetDistance2D(to, segments[i]) < eps) {
					seg2 = i;
				}
			}

			if (seg1 == seg2) {
				return Calculations.GetDistance2D(from, to);
			}
			else {
				if (seg1 < seg2) {
					distance += Calculations.GetDistance2D(from, segments[seg1].EndPoint);

					while (++seg1 < seg2) {
						distance += Calculations.GetDistance2D(segments[seg1].StartPoint, segments[seg1].EndPoint);
					}

					distance += Calculations.GetDistance2D(segments[seg1].StartPoint, to);
				}
				else {
					distance += Calculations.GetDistance2D(from, segments[seg1].StartPoint);

					while (--seg1 > seg2) {
						distance += Calculations.GetDistance2D(segments[seg1].StartPoint, segments[seg1].EndPoint);
					}

					distance += Calculations.GetDistance2D(segments[seg1].EndPoint, to);
				}

				return distance;
			}
		}
		
		public void Match(GPXTrackSegment gpx) {
			_layers = new List<CandidateGraphLayer>();

			//Find candidate points + ObservationProbability
			foreach (var gpxPoint in gpx.Nodes) {
				var candidates = FindCandidatePoints(gpxPoint).OrderByDescending(p => p.ObservationProbability);

				CandidateGraphLayer layer = new CandidateGraphLayer() { TrackPoint = gpxPoint };
				layer.Candidates.AddRange(candidates.Take(Math.Min(candidates.Count(), 5)));
				_layers.Add(layer);
			}

			// Transmissio probability
			ConnectLayers();
		}


		void ConnectLayers() {
			for (int l = 0; l < _layers.Count-1; l++) {
				for (int i = 0; i < _layers[l].Candidates.Count; i++) {
					for (int j = 0; j < _layers[l+1].Candidates.Count; j++) {
						AddConnection(_layers[l].Candidates[i], _layers[l + 1].Candidates[j]);
					}
				}
			}
		}

		void AddConnection(CandidatePoint from, CandidatePoint to) {
			CandidatesConection c = new CandidatesConection() { From = from, To = to };
			from.OutgoingConnections.Add(c);
			to.IncomingConnections.Add(c);

			c.TransmissionProbability = CalculateTransmissionProbability(c);
		}
	}
}
