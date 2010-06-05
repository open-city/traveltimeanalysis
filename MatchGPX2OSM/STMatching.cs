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

		public void Match(GPXTrackSegment gpx) {
			List<CandidateGraphLayer> layers = new List<CandidateGraphLayer>();

			//Find candidate points + ObservationProbability
			foreach (var gpxPoint in gpx.Nodes) {
				var candidates = FindCandidatePoints(gpxPoint).OrderByDescending(p => p.ObservationProbability);

				CandidateGraphLayer pc = new CandidateGraphLayer() { TrackPoint = gpxPoint };
				pc.Candidates.AddRange(candidates.Take(Math.Min(candidates.Count(), 5)));
				layers.Add(pc);
			}
		}

	}
}
