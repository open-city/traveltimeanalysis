using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GPXUtils;

namespace LK.MatchGPX2OSM {
	public class CandidateGraphLayer {
		public GPXPoint TrackPoint { get; set; }

		public List<CandidatePoint> Candidates { get; private set; }

		public CandidateGraphLayer() {
			Candidates = new List<CandidatePoint>(5);
		}
	}
}
