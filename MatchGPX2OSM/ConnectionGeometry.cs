using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.MatchGPX2OSM {
	public class ConnectionGeometry : Polyline<IPointGeo> {
		public int WayID { get; set; }
		public List<Connection> Connections { get; private set; }

		public ConnectionGeometry()
			: base() {
				Connections = new List<Connection>(2);
		}

		private BBox _bbox;
		public BBox BBox {
			get {
				if (_bbox == null) {
					_bbox = new BBox(Nodes);
					_bbox.Inflate(0.0014);
				}
				return _bbox;
			}
		}

		public IEnumerable<Segment<IPointGeo>> GetSegments() {
			for (int i = 0; i < Nodes.Count -1; i++) {
				yield return new Segment<IPointGeo>(Nodes[i], Nodes[i + 1]);
			}
		}
	}
}
