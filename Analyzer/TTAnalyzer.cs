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
				var signals = EstimateTafficSignalsDelay(travelTimes, segment);
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
			double maximalStopSpeed = 1.0;
			double maximalDistanceFromSignals = 50;

			int totalStops = 0;
			int totalStopsLength = 0;

			IPointGeo segmentStart = _map.Nodes[segment.NodeFromID];
			IPointGeo segmentEnd = _map.Nodes[segment.NodeToID];

			foreach (var travelTime in travelTimes) {
				List<double> avgSpeeds = new List<double>();
				if (travelTime.Points.Count > 0) {
					avgSpeeds.Add(Calculations.GetDistance2D(segmentStart, travelTime.Points[0]) / (travelTime.Points[0].Time - travelTime.TimeStart).TotalSeconds);

					for (int i = 0; i < travelTime.Points.Count -1; i++) {
						avgSpeeds.Add(Calculations.GetDistance2D(travelTime.Points[i], travelTime.Points[i + 1]) / (travelTime.Points[i + 1].Time - travelTime.Points[i].Time).TotalSeconds);
					}

					avgSpeeds.Add(Calculations.GetDistance2D(travelTime.Points.Last(), segmentEnd) / (travelTime.TimeEnd - travelTime.Points.Last().Time).TotalSeconds);
				}
				else {
					avgSpeeds.Add(Calculations.GetDistance2D(segmentStart, segmentEnd) / travelTime.TotalTravelTime.TotalSeconds);
				}

				int index = avgSpeeds.Count - 1;
				while (index >= 0) {
					while (index >= 0 && avgSpeeds[index] < maximalStopSpeed) {
						index--;
					}
					index--;
				}
			}

			return new TrafficSignalsDelayInfo() { Probability = travelTimes.Count() / totalStops, Length = totalStopsLength / totalStops };
		}
	}
}
