using System;
using System.Collections.Generic;
using System.Text;

namespace LK.GeoUtils.Geometry {
	/// <summary>
	/// Represents a polyline defined by IPointGeo objects
	/// </summary>
	public interface IPolyline<T> where T : IPointGeo {
		/// <summary>
		/// Gets nodes of this polyline
		/// </summary>
		IList<T> Nodes { get; }

		/// <summary>
		/// Gets nodes count
		/// </summary>
		int NodesCount { get; }
	}
}
