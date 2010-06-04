using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils;
using LK.GeoUtils.Geometry;

namespace LK.MatchGPX2OSM {
	public class Node {
		/// <summary>
		/// Creates a new Node with the specific position
		/// </summary>
		/// <param name="position">The position of the node in geographic coordinates</param>
		public Node(IPointGeo position) {
			this._connections = new List<Connection>();

			this.Position = position;
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
		public IEnumerable<Connection> Connections {
			get {
				return _connections;
			}
		}

		/// <summary>
		/// Gets or sets position of this node in geographc coordinates
		/// </summary>
		public IPointGeo Position { get; set; }

		/// <summary>
		/// Gets or sets ID
		/// </summary>
		public int ID { get; set; }
		
		/// <summary>
		/// Adds the specific connection to this Node
		/// </summary>
		/// <param name="toAdd">The connection to be added</param>
		public void AddConnection(Connection toAdd) {
			_connections.Add(toAdd);
		}

		/// <summary>
		/// Removes the connection from this node
		/// </summary>
		/// <param name="toRemove">The connection to be removed</param>
		/// <returns>true if the connection was successfully removed, otherwise returns false</returns>
		public bool RemoveConnection(Connection toRemove) {
			return this._connections.Remove(toRemove);
		}
	}
}
