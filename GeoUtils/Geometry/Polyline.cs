using System;
using System.Collections.Generic;
using System.Text;

namespace LK.GeoUtils.Geometry {
	/// <summary>
	/// Represents a polyline defined by IPointGeo objects
	/// </summary>
	public class Polyline<T> : IPolyline, IPolyline<T> where T : IPointGeo  {
		protected List<T> _nodes;

		/// <summary>
		/// Gets the list of nodes of this polyline
		/// </summary>
		/// <remarks>Implements generic IPolyline interface</remarks>
		public IList<T> Nodes {
			get { return _nodes; }
		}

		/// <summary>
		/// Gets the list of segment of this polyline
		/// </summary>
		/// <remarks>List isn't cached, it is built every time it is requested</remarks>
		public IList<Segment<T>> Segments {
			get {
				return GetSegments();
			}
		}

		/// <summary>
		/// Builds list of segments of this polyline
		/// </summary>
		/// <returns>List of segments of this polyline</returns>
		protected List<Segment<T>> GetSegments() {
			List<Segment<T>> result = new List<Segment<T>>();
			for (int i = 0; i < Nodes.Count -1; i++) {
				result.Add(new Segment<T>(Nodes[i], Nodes[i + 1]));
			}

			return result;
		}
		/// <summary>
		/// Gets the list of nodes of this polyline
		/// </summary>
		/// <remarks>Implements non-generic IPolyline interface</remarks>
		IList<IPointGeo> IPolyline.Nodes {
			get { return (IList<IPointGeo>)_nodes; }
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
