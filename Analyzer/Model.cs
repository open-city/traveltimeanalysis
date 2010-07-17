using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Analyzer {
	public class Model {
		public SegmentInfo Segment { get; set; }

		public double FreeFlowTravelTime { get; set; }
		public List<TrafficDelayInfo> TrafficDelay { get; private set; }
		public double AvgDelay { get; set; }
		public TrafficSignalsDelayInfo TrafficSignalsDelay { get; set; }

		public Model() {
			TrafficDelay = new List<TrafficDelayInfo>();
		}
	}

	public struct TrafficSignalsDelayInfo {
		public double Length;
		public double Probability;
	}
}
