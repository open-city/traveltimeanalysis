using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.MatchGPX2OSM;
using LK.GeoUtils;
using LK.OSMUtils.OSMDatabase;

namespace LK.MatchGPX2OSM {
	public class Astar {
		protected RoadGraph _graph;

		public Astar(RoadGraph graph) {
			_graph = graph;
		}

		public IList<PathSegment> FindPath(CandidatePoint from, CandidatePoint to, ref double length) {
			SortedPathList open = new SortedPathList();
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
				PartialPath current = open[0];
				open.Remove(current);
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
						open.Add(new PartialPath() { CurrentPosition = destination, Length = distance, PreviousNode = current.CurrentPosition, PreviousPath = to.Road });
					}
				}
				
				foreach (var link in current.CurrentPosition.Connections) {
					if (link.From != current.CurrentPosition) continue;
					double distance = current.Length + link.Geometry.Length;
					PartialPath expanded = null;
					if (open.Contains(link.To)) {
						if (open[link.To].Length > distance) {
							open[link.To].Length = distance;
							open[link.To].PreviousNode = current.CurrentPosition;
							open[link.To].PreviousPath = link.Geometry;
							open.Update();
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



		class SortedPathList {
			protected List<PartialPath> paths;
			protected Dictionary<Node, PartialPath> nodePath;

			public PartialPath this[int index] {
				get {
					return paths[index];
				}
			}

			public PartialPath this[Node node] {
				get {
					return nodePath[node];
				}
			}

			public int Count {
				get {
					return paths.Count;
				}
			}

			public SortedPathList() {
				paths = new List<PartialPath>();
				nodePath = new Dictionary<Node, PartialPath>();
			}

			public bool Contains(Node pathEnd) {
				return nodePath.ContainsKey(pathEnd);
			}

			public void Add(PartialPath path) {
				if (nodePath.ContainsKey(path.CurrentPosition)) {
					nodePath[path.CurrentPosition] = path;
					Update();
				}
				else {
					nodePath.Add(path.CurrentPosition, path);
					paths.Add(path);
					int index = 0;
					while (index < paths.Count && paths[index].Length < path.Length) {
						index++;
					}
					paths.Insert(index, path);
				}
			}

			public void Remove(PartialPath path) {
				nodePath.Remove(path.CurrentPosition);
				paths.Remove(path);
			}

			public void Update() {
				paths.Sort();
			}
		}
	}
}
