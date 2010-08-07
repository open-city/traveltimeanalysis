//  Travel Time Analysis project
//  Copyright (C) 2010 Lukas Kabrt
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Xunit;

using LK.Analyzer;
using LK.GPXUtils;
using LK.OSMUtils.OSMDatabase;

namespace Analyzer.Tests {
	public class AnalyzerTests {
		[Fact()]
		public void AnalyzeEstimatesFreeFlowTimeFromFastestLogs() {
			List<TravelTime> travelTimes = new List<TravelTime>();
			travelTimes.Add(new TravelTime(new SegmentInfo(), new DateTime(2010, 7, 7, 10, 00, 00), new DateTime(2010, 7, 7, 10, 00, 34)));
			travelTimes.Add(new TravelTime(new SegmentInfo(), new DateTime(2010, 7, 7, 10, 00, 00), new DateTime(2010, 7, 7, 10, 00, 32)));
			travelTimes.Add(new TravelTime(new SegmentInfo(), new DateTime(2010, 7, 7, 10, 00, 00), new DateTime(2010, 7, 7, 10, 00, 30)));
			travelTimes.Add(new TravelTime(new SegmentInfo(), new DateTime(2010, 7, 7, 10, 00, 00), new DateTime(2010, 7, 7, 10, 00, 44)));
			travelTimes.Add(new TravelTime(new SegmentInfo(), new DateTime(2010, 7, 7, 10, 00, 00), new DateTime(2010, 7, 7, 10, 00, 46)));

			OSMDB map = new OSMDB();
			map.Nodes.Add(new OSMNode(1, 1, 1));
			map.Nodes.Add(new OSMNode(2, 2, 2));

			TTAnalyzer analyzer = new TTAnalyzer(map);
			Model target = analyzer.Analyze(travelTimes, new SegmentInfo() { NodeFromID = 1, NodeToID = 2, WayID = 100 });

			// average from 3 or 10% fastest
			Assert.Equal(32, target.FreeFlowTravelTime);
		}

		[Fact()]
		public void AnalyzeDetectsStopAtTrafficSignals() {
			OSMDB track = new OSMDB();
			track.Load(new MemoryStream(TestData.osm_real_traffic_signals));

			var travelTimeAtSignals = TravelTime.FromMatchedTrack(track).First();

			OSMDB map = new OSMDB();
			map.Load(new MemoryStream(TestData.osm_traffic_signals_map));

			TTAnalyzer analyzer = new TTAnalyzer(map);
			Model target = analyzer.Analyze(new TravelTime[] { travelTimeAtSignals }, travelTimeAtSignals.Segment);

			Assert.Equal(30, target.TrafficSignalsDelay.Length);
			Assert.Equal(1.0, target.TrafficSignalsDelay.Probability);
		}

		[Fact()]
		public void AnalyzeFindTravelTimePattersInOneDay_Simulated() {
			XmlTravelTimeDB db = new XmlTravelTimeDB();
			db.Load(new MemoryStream(TestData.simulated_traffic_db_day));

			OSMDB map = new OSMDB();
			map.Nodes.Add(new OSMNode(1, 1, 1));
			map.Nodes.Add(new OSMNode(2, 1.1, 1.1));

			TTAnalyzer analyzer = new TTAnalyzer(map);
			Model target = analyzer.Analyze(db.TravelTimes, db.TravelTimesSegments.First());
		}
	}
}
