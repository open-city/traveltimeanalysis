using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LK.GeoUtils;
using LK.OSMUtils.OSMDatabase;
using LK.GeoUtils.Geometry;

namespace LK.MatchGPX2OSM {
	public class PathReconstructer {
		/// <summary>
		/// Recontructs path from the candidates points
		/// </summary>
		/// <param name="matched"></param>
		/// <returns></returns>
		public OSMDB Reconstruct(IList<CandidatePoint> matched, RoadGraph _graph) {
			AstarPathfinder pathfinder = new AstarPathfinder(_graph);
			OSMDB result = new OSMDB();
			int counter = -1;

			OSMNode node = AddNodeToPath(result, ref counter, matched[0]);
			node.Tags.Add(new OSMTag("time", matched[0].Layer.TrackPoint.Time.ToString()));

			for (int i = 0; i < matched.Count - 1; i++) {
				if (counter < -114) {
					int a = 1;
				}
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

					var points = Topology.GetNodesBetweenPoints(matched[i], matched[i + 1], wayGeometry);
					foreach (var point in points) {
						node = AddNodeToPath(result, ref counter, point);
						way.Nodes.Add(node.ID);
					}
				}
				else {
					double lenght = double.PositiveInfinity;
					// find path between matched[i] and matched[i+1]
					var paths = pathfinder.FindPath(matched[i], matched[i + 1], ref lenght).ToList();

					int lastWayId = 0;
					if (paths.Count > 0) {
						lastWayId = paths[0].Road.WayID;
						way.Tags.Add(new OSMTag("way-id", paths[0].Road.WayID.ToString()));
					}

					for (int j = 0; j < paths.Count; j++) {
						if (j > 0) {
							node = AddNodeToPath(result, ref counter, paths[j].From.MapPoint);
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

						var points = Topology.GetNodesBetweenPoints(paths[j].From.MapPoint, paths[j].To.MapPoint, paths[j].Road).ToList();
						foreach (var point in points) {
							node = AddNodeToPath(result, ref counter, point);
							way.Nodes.Add(node.ID);
						}
					}
				}
				//if (Calculations.GetDistance2D(node, matched[i + 1]) > Calculations.EpsLength) {
					node = AddNodeToPath(result, ref counter, matched[i + 1]);
					way.Nodes.Add(node.ID);
				//}
				node.Tags.Add(new OSMTag("time", matched[i + 1].Layer.TrackPoint.Time.ToString()));
			}

			var ways = result.Ways.ToList();

			//Segment<OSMNode> lastSegment = new Segment<OSMNode>(result.Nodes[ways[0].Nodes[ways[0].Nodes.Count - 2]], result.Nodes[ways[0].Nodes[ways[0].Nodes.Count - 1]]);
			//double lastAngle = Calculations.GetBearing(lastSegment.StartPoint, lastSegment.EndPoint);

			//for (int i = 1; i < ways.Count; i++) {
			//  Segment<OSMNode> segment = new Segment<OSMNode>(result.Nodes[ways[i].Nodes[0]], result.Nodes[ways[i].Nodes[1]]);
			//  double angle = Calculations.GetBearing(segment.StartPoint, segment.EndPoint);

			//  if (Math.Abs(180 - Math.Abs(lastAngle - angle)) < 1) {
			//    if (i < ways.Count - 1 && Calculations.GetDistance2D(segment.StartPoint, segment.EndPoint) < 50) {
			//      ways[i + 1].Nodes[0] = ways[i].Nodes[0];
			//      result.Ways.Remove(ways[i]);
			//    }
			//  }
			//}

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
			if (node is OSMNode) {
				result.Tags.Add(new OSMTag("node-id", ((OSMNode)node).ID.ToString()));
			}

			db.Nodes.Add(result);

			return result;
		}


	}
}
