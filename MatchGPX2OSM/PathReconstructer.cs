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
		public OSMDB Reconstruct(IList<CandidatePoint> matched) {
			_db = new OSMDB();
			_dbCounter = -1;

			if (matched.Count == 0)
				return _db;

			OSMNode node = AddNode(matched[0].MapPoint, matched[0].Layer.TrackPoint.Time);

			for (int i = 0; i < matched.Count - 1; i++) {
				ConnectionGeometry wayGeometry = null;
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

						if (j < pathSegments.Count-1) {
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

			_db.Ways.Add(result);

			return result;
		}
	}
}
