using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GPXUtils;

namespace LK.MatchGPX2OSM {
	/// <summary>
	/// Represents set of the candidate points for the given GPX point
	/// </summary>
	public class CandidateGraphLayer {
		/// <summary>
		/// Gets or sets point of the GPX track
		/// </summary>
		public GPXPoint TrackPoint { get; set; }

		/// <summary>
		/// Gets the collection of candidate points for the TrackPoint
		/// </summary>
		public List<CandidatePoint> Candidates { get; private set; }

		/// <summary>
		/// Creates a new instance of the CandidateGraphLayer
		/// </summary>
		public CandidateGraphLayer() {
			Candidates = new List<CandidatePoint>(STMatching.MaxCandidatesCount);
		}
	}
}
