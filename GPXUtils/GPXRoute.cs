using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.GPXUtils {
	/// <summary>
	/// Represents a route
	/// </summary>
	public class GPXRoute : Polyline<GPXPoint> {
		/// <summary>
		/// Gets or sets the name of this route
		/// </summary>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// Creates a new, empty instance of GPXRoute
		/// </summary>
		public GPXRoute() {
		}

		/// <summary>
		/// Creates a new instance of GPXRoute with specific name
		/// </summary>
		/// <param name="name">The name of this GPXRoute</param>
		public GPXRoute(string name) {
			Name = name;
		}
	}
}
