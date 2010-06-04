using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.MatchGPX2OSM {
	/// <summary>
	/// Represents a directed connection (an edge in the roadgraph) between two nodes.
	/// </summary>
	public class Connection {
		/// <summary>
		/// Create a new connection with the specific end nodes
		/// </summary>
		/// <param name="from">The node, where this connection starts</param>
		/// <param name="to">The noe where this connection ends</param>
		public Connection(Node from, Node to) {
			this.From = from;
			this.To = to;
		}

		/// <summary>
		/// Gets or sets node, where this connection starts
		/// </summary>
		public Node From { get; set; }

		/// <summary>
		/// Gets or set node wher this connection ends
		/// </summary>
		public Node To { get; set; }

		/// <summary>
		/// Gets or sets IPolyline object representing geometry of the connection
		/// </summary>
		public IPolyline<IPointGeo> Geometry { get; set; }

		/// <summary>
		/// Gets or sets maximal speed on this connection
		/// </summary>
		public double Speed { get; set; }

		/// <summary>
		/// Gets or sets ID
		/// </summary>
		public int ID { get; set; }
	}
}
