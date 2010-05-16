using System;
using System.Collections.Generic;
using System.Text;

namespace LK.GeoUtils.Geometry {
	/// <summary>
	/// Represents a polyline defined by IPointGeo objects
	/// </summary>
	public class Polyline<T> : IPolyline<T> where T : IPointGeo  {
		protected List<T> _nodes;

		/// <summary>
		/// Gets the list of nodes of this polyline
		/// </summary>
		public IList<T> Nodes {
			get { return _nodes; }
		}

		/// <summary>
		/// Gets nodes count
		/// </summary>
		public int NodesCount {
			get { return _nodes.Count; }
		}

		/// <summary>
		/// Creates a new, empty instance of the polyline
		/// </summary>
		public Polyline() {
			_nodes = new List<T>();
		}

	}
}
