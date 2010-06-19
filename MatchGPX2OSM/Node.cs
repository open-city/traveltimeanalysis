using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils;
using LK.GeoUtils.Geometry;

namespace LK.MatchGPX2OSM {
	/// <summary>
	/// Represents a node in the routable road graph
	/// </summary>
	public class Node {
		/// <summary>
		/// Creates a new Node with the specific position
		/// </summary>
		/// <param name="mapPoint">The position of the node in geographic coordinates</param>
		public Node(IPointGeo mapPoint) {
			this._connections = new List<Connection>();
			this.MapPoint = mapPoint;
		}

		/// <summary>
		/// Creates a new Node
		/// </summary>
		public Node() {
			this._connections = new List<Connection>();
		}

		private List<Connection> _connections;
		/// <summary>
		/// Gets the collection of all connections going to or from this node
		/// </summary>
		public IList<Connection> Connections {
			get {
				return _connections;
			}
		}

		/// <summary>
		/// Gets or sets position of this node in geographic coordinates
		/// </summary>
		public IPointGeo MapPoint { get; set; }
	}
}
