using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.MatchGPX2OSM {
	public class RoadGraph {
		private List<Node> _nodes;
		/// <summary>
		/// Gets collection of all nodes in this graph
		/// </summary>
		public IEnumerable<Node> Nodes {
			get {
				return _nodes;
			}
		}

		private List<Connection> _connections;
		/// <summary>
		/// Gets collection of all enges in this graph
		/// </summary>
		public IEnumerable<Connection> Connections {
			get {
				return _connections;
			}
		}

		/// <summary>
		/// Creates a new RoadGraph
		/// </summary>
		public RoadGraph() {
			_nodes = new List<Node>();
			_connections = new List<Connection>();
		}
	}
}
