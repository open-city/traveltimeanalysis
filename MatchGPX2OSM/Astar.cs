using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.MatchGPX2OSM;
using LK.GeoUtils;
using LK.OSMUtils.OSMDatabase;

namespace LK.MatchGPX2OSM {
	public class Astar {
		public int nodesCount = 0;
		public int runs = 0;

		protected RoadGraph _graph;

		public Astar(RoadGraph graph) {
			_graph = graph;
		}

		public IList<PathSegment> FindPath(CandidatePoint from, CandidatePoint to, ref double length) {
			PartialPathList open = new PartialPathList();
			Dictionary<Node, PartialPath> close = new Dictionary<Node, PartialPath>();

			List<Node> target = new List<Node>();
			foreach (var targetConnections in to.Road.Connections) {
				target.Add(targetConnections.From);
			}
			Node destination = new Node() { MapPoint = to };
			

			foreach (var connection in from.Road.Connections) {
				PartialPath path = new PartialPath();
				path.CurrentPosition = connection.To;
				path.Length = Calculations.GetPathLength(from, path.CurrentPosition.MapPoint, connection.Geometry);
				path.PreviousPath = connection.Geometry;
				open.Add(path);
			}

			while (open.Count > 0) {
				PartialPath current = open.RemoveTop();
				if (close.ContainsKey(current.CurrentPosition) == false) {
					close.Add(current.CurrentPosition, current);
				}
				else {
					close[current.CurrentPosition] = current;
				}

				if (Calculations.GetDistance2D(current.CurrentPosition.MapPoint, destination.MapPoint) < Calculations.EpsLength) {		
					length = current.Length;
					List<PathSegment> result = new List<PathSegment>();

					while (current.PreviousNode != null) {
						result.Add(new PathSegment() { From = current.PreviousNode, To = current.CurrentPosition, Road = current.PreviousPath });
						current = close[current.PreviousNode];
					}
					result.Add(new PathSegment() { From = new Node(from), To = current.CurrentPosition, Road = current.PreviousPath });

					result.Reverse();
					runs++;
					nodesCount += close.Count + open.Count;

					return result;
				}

				if (target.Contains(current.CurrentPosition)) {
					double distance = current.Length + Calculations.GetPathLength(current.CurrentPosition.MapPoint, destination.MapPoint, to.Road);
					if (open.Contains(destination)) {
						if (open[destination].Length > distance) {
							open[destination].Length = distance;
							open[destination].PreviousNode = current.CurrentPosition;
						}
					}
					else {
						PartialPath p = new PartialPath() { CurrentPosition = destination, Length = distance, PreviousNode = current.CurrentPosition, PreviousPath = to.Road };
						open.Add(p);
					}
				}
				
				foreach (var link in current.CurrentPosition.Connections) {
					if (link.From != current.CurrentPosition) continue;
					double distance = current.Length + link.Geometry.Length;
					PartialPath expanded = null;
					if (open.Contains(link.To)) {
						PartialPath p = open[link.To];
						if (p.Length > distance) {
							p.PreviousNode = current.CurrentPosition;
							p.PreviousPath = link.Geometry;
							open.Update(p, distance);
						}
					}

					else if (close.ContainsKey(link.To)) {
						if (close[link.To].Length > distance) {
							close[link.To].Length = distance;
							close[link.To].CurrentPosition = current.CurrentPosition;
							close[link.To].PreviousPath = link.Geometry;
						}
					}
					else {
						expanded = new PartialPath() { Length = distance, CurrentPosition = link.To, PreviousNode = current.CurrentPosition, PreviousPath = link.Geometry};
						open.Add(expanded);
					}
				}
			}

			return null;
		}
	}
}
