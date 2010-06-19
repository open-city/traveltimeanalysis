using System;
using System.Collections.Generic;
using System.Text;

namespace LK.GeoUtils.Geometry {
	/// <summary>
	/// Represents a generic interface for polyline
	/// </summary>
	public interface IPolyline<T> where T : IPointGeo {
		/// <summary>
		/// Gets nodes of this polyline
		/// </summary>
		IList<T> Nodes { get; }

		/// <summary>
		/// Gets segments of this polyline
		/// </summary>
		IList<Segment<T>> Segments { get; }

		/// <summary>
		/// Gets the length of the IPolyline in meters
		/// </summary>
		double Length { get; }

		/// <summary>
		/// Gets nodes count
		/// </summary>
		int NodesCount { get; }
	}

	/// <summary>
	/// Represents a non-generic interface for polyline
	/// </summary>
	public interface IPolyline {
		/// <summary>
		/// Gets nodes of this polyline
		/// </summary>
		IList<IPointGeo> Nodes { get; }

		/// <summary>
		/// Gets nodes count
		/// </summary>
		int NodesCount { get; }
	}
}
