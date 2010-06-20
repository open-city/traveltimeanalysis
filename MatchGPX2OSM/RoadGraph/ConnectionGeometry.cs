using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.MatchGPX2OSM {
	//Represents a shape of the connection
	public class ConnectionGeometry : Polyline<IPointGeo> {
		/// <summary>
		/// Gets or sets OSM ID of the original way
		/// </summary>
		public int WayID { get; set; }

		/// <summary>
		/// Gets the list of connections that shares this shape
		/// </summary>
		public IList<Connection> Connections { get; private set; }

		/// <summary>
		/// Creates a new Connection geometry object
		/// </summary>
		public ConnectionGeometry()
			: base() {
				Connections = new List<Connection>(2);
		}

		private BBox _bbox;
		/// <summary>
		/// Gets bounding box of this connection geometry
		/// </summary>
		/// <remarks>BBox is computed when it's accessed for the first time and it's cached</remarks>
		public BBox BBox {
			get {
				if (_bbox == null) {
					_bbox = new BBox(Nodes);
				}
				return _bbox;
			}
		}
	}
}
