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
			SortedPathList close = new SortedPathList();

			List<Node> target = new List<Node>();
			foreach (var targetConnections in to.Road.Connections) {
				target.Add(targetConnections.From);
			}
			Node destination = new Node() { Position = to };
			

			foreach (var connection in from.Road.Connections) {
				Path path = new Path();
				path.Position = connection.To;
				path.PathLength = Calculations.GetPathLength(from, path.Position.Position, connection.Geometry);
				path.PreviousPath = connection.Geometry;
				open.Add(path);
			}

			while (open.Count > 0) {
				Path current = open[0];
				open.Remove(current);
				close.Add(current);

				if (Calculations.GetDistance2D(current.Position.Position, destination.Position) < Calculations.EpsLength) {		
					length = current.PathLength;
					List<PathSegment> result = new List<PathSegment>();

					while (current.PreviousNode != null) {
						result.Add(new PathSegment() { From = current.PreviousNode, To = current.Position, Road = current.PreviousPath });
						current = close[current.PreviousNode];
					}
					result.Add(new PathSegment() { From = new Node(from), To = current.Position, Road = current.PreviousPath });

					result.Reverse();
					return result;
				}

				if (target.Contains(current.Position)) {
					double distance = current.PathLength + Calculations.GetPathLength(current.Position.Position, destination.Position, to.Road);
					if (open.Contains(destination)) {
						if (open[destination].PathLength > distance) {
							open[destination].PathLength = distance;
							open[destination].PreviousNode = current.Position;
						}
					}
					else {
						open.Add(new Path() { Position = destination, PathLength = distance, PreviousNode = current.Position, PreviousPath = to.Road });
					}
				}
				
				foreach (var link in current.Position.Connections) {
					if (link.From != current.Position) continue;
					double distance = current.PathLength + link.Geometry.Length; //Calculations.GetLength(link.Geometry);
					Path expanded = null;
					if (open.Contains(link.To)) {
						if (open[link.To].PathLength > distance) {
							open[link.To].PathLength = distance;
							open[link.To].PreviousNode = current.Position;
							open[link.To].PreviousPath = link.Geometry;
							open.Update();
						}
					}
					else if (close.Contains(link.To)) {
						if (close[link.To].PathLength > distance) {
							close[link.To].PathLength = distance;
							close[link.To].Position = current.Position;
							close[link.To].PreviousPath = link.Geometry;
						}
					}
					else {
						expanded = new Path() { PathLength = distance, Position = link.To, PreviousNode = current.Position, PreviousPath = link.Geometry};
						open.Add(expanded);
					}
				}
			}

			return null;
		}

		class Path : IComparer<Path>, IComparable {
			public Node Position;
			public Node PreviousNode;
			public ConnectionGeometry PreviousPath;
			public double PathLength;

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
				return x.PathLength.CompareTo(y.PathLength);
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
				if (nodePath.ContainsKey(path.Position)) {
					nodePath[path.Position] = path;
					Update();
				}
				else {
					nodePath.Add(path.Position, path);
					paths.Add(path);
					int index = 0;
					while (index < paths.Count && paths[index].PathLength < path.PathLength) {
						index++;
					}
					paths.Insert(index, path);
				}
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
