using System;
using System.Collections.Generic;
using System.Text;

namespace LK.GeoUtils.Geometry {
	/// <summary>
	/// Defines point on the Earth surface
	/// </summary>
	public interface IPointGeo {
		/// <summary>
		/// Gets or sets latitude of this point (north - positive value, south - negative value)
		/// </summary>
		double Latitude { get; set; }

		/// <summary>
		/// Gets or sets the longitude of this point (north - positive value, south - negative value)
		/// </summary>
		double Longitude {get; set;}

		/// <summary>
		/// Gets or sets the elevation aboce MSL in meters of this point
		/// </summary>
		double Elevation { get; set; }
	}
}
