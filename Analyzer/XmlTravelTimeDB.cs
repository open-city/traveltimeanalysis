using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Analyzer {
	public class XmlTravelTimeDB : ITravelTimesDB {
		Dictionary<SegmentInfo, List<TravelTime>> _storage;

		public IEnumerable<TravelTime> TravelTimes {
			get {
				return _storage.Values.SelectMany(segTravelTimes => segTravelTimes);
			}
		}

		public IEnumerable<TravelTime> GetTravelTimes(SegmentInfo segment) {
			return _storage[segment];
		}

		public void AddTravelTime(TravelTime toAdd) {
			if (_storage.ContainsKey(toAdd.Segment) == false) {
				_storage.Add(toAdd.Segment, new List<TravelTime>());
			}

			_storage[toAdd.Segment].Add(toAdd);
		}

		public bool RemoveTravelTime(TravelTime toRemove) {
			if (_storage.ContainsKey(toRemove.Segment)) {
				return _storage[toRemove.Segment].Remove(toRemove);
			}

			return false;
		}

		public XmlTravelTimeDB() {
			_storage = new Dictionary<SegmentInfo, List<TravelTime>>();
		}
	}
}
