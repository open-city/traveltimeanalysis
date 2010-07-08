using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Analyzer {
	public class Model {
		public SegmentInfo Segment { get; set; }

		public double FreeFlowTravelTime { get; set; }
		public double DelayTraffic { get; set; }
		public double DelaySignals { get; set; }
		public double SignalsProbability { get; set; }
	}
}
