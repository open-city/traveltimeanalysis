using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LK.Analyzer {
	public interface ITravelTimesDB {
		IEnumerable<TravelTime> TravelTimes { get; }
		IEnumerable<SegmentInfo> TravelTimesSegments { get; }
		IEnumerable<TravelTime> GetTravelTimes(SegmentInfo segment);

		void AddTravelTime(TravelTime toAdd);
		bool RemoveTravelTime(TravelTime toRemove);
	}
}
