using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.OSMUtils.OSMDatabase {
	/// <summary>
	/// Represents way in the OSM database.
	/// </summary>
	public class OSMWay : OSMObject {
		/// <summary>
		/// Creates a new OSMWay with the scpecific ID.
		/// </summary>
		/// <param name="id">ID of the OSMWay.</param>
		public OSMWay(int id)
			: base(id) {
				_nodes = new List<int>();
		}

		/// <summary>
		/// Creates a new OSMWay with the scpecific ID and list of nodes
		/// </summary>
		/// <param name="id">ID of the OSMWay.</param>
		/// <param name="nodes">Nodes of this OSMWay</param>
		public OSMWay(int id, IList<int> nodes)
			: base(id) {
			_nodes = new List<int>(nodes);
		}

		protected List<int> _nodes;
		/// <summary>
		/// Gets list of node IDs, that forms the way
		/// </summary>
		public IList<int> Nodes {
			get {
				return _nodes;
			}
		}

		/// <summary>
		/// Gets bool value indicating whether the way is closed.
		/// </summary>
		/// <remarks>A closed way must have at least 3 nodes.</remarks>
		public bool IsClosed {
			get {
				if (_nodes.Count < 3) {
					return false;
				}

				return _nodes.First() == _nodes.Last();
			}
		}
	}
}
