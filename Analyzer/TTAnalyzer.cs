using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Analyzer {
	public class TTAnalyzer {
		public Model Analyze(IEnumerable<TravelTime> travelTimes) {
			Model result = new Model();
			result.Segment = travelTimes.First().Segment;

			result.FreeFlowTravelTime = EstimateFreeFlowTime(travelTimes);

			return result;
		}

		double EstimateFreeFlowTime(IEnumerable<TravelTime> travelTimes) {
			double PercentageFastest = 10.0;
			int MinimalCount = 3;

			int desiredCount = (int)Math.Max(travelTimes.Count() * PercentageFastest / 100.0, MinimalCount);
			int count = Math.Min(travelTimes.Count(), desiredCount);

			var toEstimate = travelTimes.OrderBy(tt => tt.TotalTravelTime).Take(count);

			return toEstimate.Sum(tt => tt.TotalTravelTime.TotalSeconds) / count;
		}
	}
}
