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

		public IList<Node> FindPath(CandidatePoint from, CandidatePoint to, ref double length) {
			SortedPathList open = new SortedPathList();
			SortedPathList close = new SortedPathList();

			List<Node> target = new List<Node>();
			foreach (var targetConnections in to.Road.Connections) {
				target.Add(targetConnections.From);
			}
			Node destination = new Node() { Position = to };
			

			foreach (var connection in from.Road.Connections) {
				Path path = new Path();
				path.Position = connection.To;
				path.PathDistance = Calculations.GetPathLength(from, path.Position.Position, connection.Geometry);

				open.Add(path);
			}

			while (open.Count > 0) {
				Path current = open[0];
				open.Remove(current);
				close.Add(current);

				if (current.Position == destination) {
					length = current.PathDistance;
					
					List<Node> result = new List<Node>();
					result.Add(current.Position);
					while (current.PreviousNode != null) {
						result.Add(current.PreviousNode);
						current = close[current.PreviousNode];
					}

					return result;
				}

				if (target.Contains(current.Position)) {
					double distance = current.PathDistance + Calculations.GetPathLength(current.Position.Position, destination.Position, to.Road);
					if (open.Contains(destination)) {
						if (open[destination].PathDistance > distance) {
							open[destination].PathDistance = distance;
							open[destination].PreviousNode = current.Position;
						}
					}
					else {
						open.Add(new Path() { Position = destination, PathDistance = distance, PreviousNode = current.Position });
					}
				}
				
				foreach (var link in current.Position.Connections) {
					if (link.From != current.Position) continue;
					double distance = current.PathDistance + Calculations.GetLength(link.Geometry);
					Path expanded = null;
					if (open.Contains(link.To)) {
						if (open[link.To].PathDistance > distance) {
							open[link.To].PathDistance = distance;
							open[link.To].PreviousNode = current.Position;
							open.Update();
						}
					}
					else if (close.Contains(link.To)) {
						if (close[link.To].PathDistance > distance) {
							close[link.To].PathDistance = distance;
							close[link.To].Position = current.Position;
						}
					}
					else {
						expanded = new Path() { PathDistance = distance, Position = link.To, PreviousNode = current.Position };
						open.Add(expanded);
					}
				}
			}

			return null;
		}

		class Path : IComparer<Path>, IComparable {
			public Node Position;
			public Node PreviousNode;
			public double PathDistance;

			public override bool Equals(object obj) {
				if (obj is Path) {
					Path other = (Path)obj;
					return this.Position.Equals(other.Position);
				}
				return base.Equals(obj);
			}

			public override int GetHashCode() {
				return this.Position.GetHashCode();
			}
			#region IComparer<Path> Members

			public int Compare(Path x, Path y) {
				return x.PathDistance.CompareTo(y.PathDistance);
			}

			#endregion

			#region IComparable Members

			public int CompareTo(object obj) {
				if (obj is Path) {
					Path other = (Path)obj;
					return Compare(this, other);
				}
				return 0;
			}

			#endregion
		}

		class SortedPathList {
			protected List<Path> paths;
			protected Dictionary<Node, Path> nodePath;

			public Path this[int index] {
				get {
					return paths[index];
				}
			}

			public Path this[Node node] {
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
				paths = new List<Path>();
				nodePath = new Dictionary<Node, Path>();
			}

			public bool Contains(Node pathEnd) {
				return nodePath.ContainsKey(pathEnd);
			}

			public void Add(Path path) {
				nodePath.Add(path.Position, path);
				int index = 0;
				while (index < paths.Count && paths[index].PathDistance < path.PathDistance) {
					index++;
				}
				paths.Insert(index, path);
			}

			public void Remove(Path path) {
				nodePath.Remove(path.Position);
				paths.Remove(path);
			}

			public void Update() {
				paths.Sort();
			}
		}
	}
}
