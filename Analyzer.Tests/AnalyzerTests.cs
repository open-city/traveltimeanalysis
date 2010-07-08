using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.Analyzer;
using LK.GPXUtils;

namespace Analyzer.Tests {
	public class AnalyzerTests {
		[Fact()]
		public void AnalyzeEstimatesFreeFlowTimeFromFastestLogs() {
			List<TravelTime> travelTimes = new List<TravelTime>();
			travelTimes.Add(new TravelTime(new SegmentInfo(), new DateTime(2010, 7, 7, 10, 00, 00), new DateTime(2010, 7, 7, 10, 00, 34), new GPXPoint[] {} ));
			travelTimes.Add(new TravelTime(new SegmentInfo(), new DateTime(2010, 7, 7, 10, 00, 00), new DateTime(2010, 7, 7, 10, 00, 32), new GPXPoint[] { }));
			travelTimes.Add(new TravelTime(new SegmentInfo(), new DateTime(2010, 7, 7, 10, 00, 00), new DateTime(2010, 7, 7, 10, 00, 30), new GPXPoint[] { }));
			travelTimes.Add(new TravelTime(new SegmentInfo(), new DateTime(2010, 7, 7, 10, 00, 00), new DateTime(2010, 7, 7, 10, 00, 44), new GPXPoint[] { }));
			travelTimes.Add(new TravelTime(new SegmentInfo(), new DateTime(2010, 7, 7, 10, 00, 00), new DateTime(2010, 7, 7, 10, 00, 46), new GPXPoint[] { }));

			TTAnalyzer analyzer = new TTAnalyzer();
			Model target = analyzer.Analyze(travelTimes);

			// average from 3 or 10% fastest
			Assert.Equal(32, target.FreeFlowTravelTime);
		}
	}
}
