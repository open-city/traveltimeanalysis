using System;
using System.Collections.Generic;
using System.Text;

using LK.GeoUtils.Geometry;

namespace OSMUtils.OSMDatabase {
	/// <summary>
	/// Represents node in the OSM database.
	/// </summary>
	public class OSMNode : OSMObject, IPointGeo {
		private double _latitude;
		/// <summary>
		/// Gets or sets the latitude of the node
		/// </summary>
		public double Latitude {
			get { return _latitude; }
			set { _latitude = value; }
		}

		private double _longitude;
		/// <summary>
		/// Gets or sets the longitude of the node
		/// </summary>
		public double Longitude {
			get { return _longitude; }
			set { _longitude = value; }
		}

		/// <summary>
		/// Creates a new OSMNode with the specified ID and coordinates
		/// </summary>
		/// <param name="id">The ID of the node</param>
		/// <param name="latitude">The latitude of the point</param>
		/// <param name="longitude">The longitude of the point</param>
		public OSMNode(int id, double latitude, double longitude)
			: base(id) {
				_latitude = latitude;
				_longitude = longitude;
		}

		/// <summary>
		/// Creates a new OSMNode, ID is set to the int.MinValue
		/// </summary>
		public OSMNode()
			: base(int.MinValue) {
		}
		
		#region IPointGeo Members

		/// <summary>
		/// Gets or sets the elevation of the node
		/// </summary>
		/// <remarks>Isn't supported right now, returns 0.</remarks>
		public double Elevation {
			get {	return 0;	}
			set {	;}
		}

		#endregion
	}
}
