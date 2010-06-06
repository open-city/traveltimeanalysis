using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.MatchGPX2OSM {
	public class CandidatePoint : IPointGeo {
		/// <summary>
		/// Gets or sets latitude of this point (north - positive value, south - negative value)
		/// </summary>
		public double Latitude { get; set; }

		/// <summary>
		/// Gets or sets the longitude of this point (east - positive value, west - negative value)
		/// </summary>
		public double Longitude { get; set; }

		/// <summary>
		/// Gets or sets the elevation aboce MSL in meters of this point
		/// </summary>
		public double Elevation { get; set; }

		public ConnectionGeometry Road { get; set; }

		public double ObservationProbability { get; set; }

		public List<CandidatesConection> OutgoingConnections { get; private set; }
		public List<CandidatesConection> IncomingConnections { get; private set; }

		public CandidatePoint() {
			OutgoingConnections = new List<CandidatesConection>();
			IncomingConnections = new List<CandidatesConection>();
		}
	}
}
