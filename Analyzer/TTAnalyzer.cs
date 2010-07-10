using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils;
using LK.GeoUtils.Geometry;
using LK.OSMUtils.OSMDatabase;

namespace LK.Analyzer {
	public class TTAnalyzer {
		OSMDB _map;

		public TTAnalyzer(OSMDB map) {
			_map = map;
		}
		
		public Model Analyze(IEnumerable<TravelTime> travelTimes, SegmentInfo segment) {
			Model result = new Model();
			result.Segment = travelTimes.First().Segment;

			result.FreeFlowTravelTime = EstimateFreeFlowTime(travelTimes);
			if (_map.Nodes[segment.NodeToID].Tags.ContainsTag("highway") && _map.Nodes[segment.NodeToID].Tags["highway"].Value == "traffic_signals") {
				result.TrafficSignalsDelay = EstimateTafficSignalsDelay(travelTimes, segment);
			}

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

		TrafficSignalsDelayInfo EstimateTafficSignalsDelay(IEnumerable<TravelTime> travelTimes, SegmentInfo segment) {
			int totalStops = travelTimes.Where(tt => tt.Stops.Count > 0).Count();
			double totalStopsLength = travelTimes.Where(tt => tt.Stops.Count > 0).Sum(tt => tt.Stops.Last().Duration.TotalSeconds);

			if (totalStops > 0)
				return new TrafficSignalsDelayInfo() { Probability = (double)totalStops / travelTimes.Count(), Length = totalStopsLength / totalStops };
			else
				return new TrafficSignalsDelayInfo() { Probability = 0, Length = 0 };
		}
	}
}
