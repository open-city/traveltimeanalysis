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

			BBox gpxPtBBox = new BBox();
			gpxPtBBox.ExtendToCover(gpxPt);
			gpxPtBBox.Inflate(0.0014);

			foreach (var road in _graph.ConnectionGeometries) {
				if (Topology.Intersects(gpxPtBBox, road.BBox)) {
					PointGeo projectedPoint = Topology.ProjectPoint(gpxPt, road);
					result.Add(new CandidatePoint() { Latitude = projectedPoint.Latitude, Longitude = projectedPoint.Longitude, Road = road });
				}
			}

			return result;
		}
	}
}
