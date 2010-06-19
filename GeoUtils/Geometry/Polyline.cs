using System;
using System.Collections.Generic;
using System.Text;

using LK.CommonLib.Collections;

namespace LK.GeoUtils.Geometry {
	/// <summary>
	/// Represents a polyline defined by IPointGeo objects
	/// </summary>
	public class Polyline<T> : IPolyline<T> where T : IPointGeo  {
		protected ObservableList<T> _nodes;

		/// <summary>
		/// Gets the list of nodes of this polyline
		/// </summary>
		public IList<T> Nodes {
			get { return _nodes; }
		}

		/// <summary>
		/// Gets the list of segment of this polyline
		/// </summary>
		List<Segment<T>> _segments;
		public IList<Segment<T>> Segments {
			get {
				if (_segments == null) {
					_segments = GetSegments();
				}
				return _segments;
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
		/// Gets nodes count
		/// </summary>
		public int NodesCount {
			get { return _nodes.Count; }
		}

		private double _length;
		/// <summary>
		/// Gets the length of the Polyline in meters
		/// </summary>
		public double Length {
			get {
				if (double.IsNaN(_length)) {
					_length = Calculations.GetLength((IPolyline<IPointGeo>)this);
				}

				return _length;
			}
		}

		protected void InvalidateComputedProperties(object sender) {
			_length = double.NaN;
			_segments = null;
		}

		/// <summary>
		/// Creates a new, empty instance of the polyline
		/// </summary>
		public Polyline() {
			_nodes = new ObservableList<T>();
			_nodes.ListContentChanged += new ListContentChangedHandler(InvalidateComputedProperties);

			InvalidateComputedProperties(null);
		}


	}
}
