using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.MatchGPX2OSM {
	/// <summary>
	/// Represents connection between two candidate points 
	/// </summary>
	public class CandidatesConection {
		public CandidatePoint From { get; set; }
		public CandidatePoint To { get; set; }

		public double TransmissionProbability { get; set; }
	}
}
