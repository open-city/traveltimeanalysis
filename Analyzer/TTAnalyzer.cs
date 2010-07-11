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

			AnalyzeTrafficDelay(travelTimes, result);

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

		class TravelTimeDelay {
			public TravelTime TravelTime { get; set; }
			public double Delay { get; set; }
		}

		void AnalyzeTrafficDelay(IEnumerable<TravelTime> travelTimes, Model model) {
			List<TravelTimeDelay> delays = new List<TravelTimeDelay>();
			foreach (var traveltime in travelTimes) {
				double delay = 0;
				if (traveltime.Stops.Count > 0)
					delay = traveltime.TotalTravelTime.TotalSeconds - model.FreeFlowTravelTime - traveltime.Stops.Last().Duration.TotalSeconds;
				else
					delay = traveltime.TotalTravelTime.TotalSeconds - model.FreeFlowTravelTime;

				delay = Math.Max(0, delay);
				delays.Add(new TravelTimeDelay() { TravelTime = traveltime, Delay = delay });
			}

			DBScan<TravelTimeDelay> clusterAnalyzer = new DBScan<TravelTimeDelay>(new DBScan<TravelTimeDelay>.FindNeighbours(FindNeighbours));
			var clusters = clusterAnalyzer.ClusterAnalysis(delays, 2);
		}

		List<TravelTimeDelay> FindNeighbours(TravelTimeDelay target, IList<TravelTimeDelay> items) {
			double sigma = items.Sum(item => item.Delay * item.Delay) / items.Count -
										Math.Pow(items.Sum(item => item.Delay) / items.Count, 2);
			//double eps = 0.1 * items.Sum(item => item.Delay) / items.Count;
			double eps = 0.3 * Math.Sqrt(sigma);
			List<TravelTimeDelay> result = new List<TravelTimeDelay>();
			for (int i = 0; i < items.Count; i++) {
				if (items[i] != target && Math.Abs(target.Delay - items[i].Delay) < eps)
					result.Add(items[i]);
			}

			return result;
		}

	}
}
