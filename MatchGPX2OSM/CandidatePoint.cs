using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils.Geometry;

namespace LK.MatchGPX2OSM {
	/// <summary>
	/// Represents candidate point during map-matching
	/// </summary>
	public class CandidatePoint {
		public IPointGeo MapPoint { get; set; }
		///// <summary>
		///// Gets or sets latitude of this point (north - positive value, south - negative value)
		///// </summary>
		//public double Latitude { get; set; }

		///// <summary>
		///// Gets or sets the longitude of this point (east - positive value, west - negative value)
		///// </summary>
		//public double Longitude { get; set; }

		/// <summary>
		/// Gets or sets the elevation above MSL in meters of this point
		/// </summary>
		public double Elevation { get; set; }

		/// <summary>
		/// Gets or sets ConnectionGeometry that represents the road on which this candidate point lies
		/// </summary>
		public ConnectionGeometry Road { get; set; }

		/// <summary>
		/// Gets or sets Segmen that represents the part of the road on which this candidate point lies
		/// </summary>
		public Segment<IPointGeo> RoadSegment { get; set; }

		/// <summary>
		/// Gets or sets Observation probability
		/// </summary>
		public double ObservationProbability { get; set; }

		/// <summary>
		/// Gets or sets layer in that this candidate point lies
		/// </summary>
		public CandidateGraphLayer Layer { get; set; }

		public List<CandidatesConnection> OutgoingConnections { get; private set; }
		public List<CandidatesConnection> IncomingConnections { get; private set; }

		/// <summary>
		/// Creates a new instance of the Candidate point
		/// </summary>
		public CandidatePoint() {
			OutgoingConnections = new List<CandidatesConnection>();
			IncomingConnections = new List<CandidatesConnection>();
		}

		private double _highestProbability = double.NegativeInfinity;
		/// <summary>
		/// Gets or sets highest probability for this candidate point during the candidates matching phase
		/// </summary>
		public double HighestProbability {
			get {
				return _highestProbability;
			}
			set {
				_highestProbability = value;
			}
		}

		/// <summary>
		/// Gets the CandidatePoint from the previous layer that's connection with this point has the highest probability
		/// </summary>
		public CandidatePoint HighesScoreParent;
	}
}
