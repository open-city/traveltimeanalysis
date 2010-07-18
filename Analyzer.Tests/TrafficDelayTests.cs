using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.Analyzer;

namespace Analyzer.Tests {
	public class TrafficDelayTests {
		//[Fact()]
		//public void ConstructorThrowsExceptionIfTimeResolutionModIsNotZero() {
		//  TrafficDelayInfo[] dummyData = new TrafficDelayInfo[1];

		//  Assert.Throws<ArgumentException>(new Assert.ThrowsDelegate(() => new TrafficDelayMap(139)));
		//}

		//[Fact()]
		//public void GetTrafficDelaysRetursSingleObjectAlignedToTheTimeResolution_SingleTimeBin() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(0, 12, 0), new TimeSpan(0, 13, 0), LK.Analyzer.DayOfWeek.Monday, 10);

		//  var target = tdm.GetDelays();
			
		//  TrafficDelayInfo expected = new TrafficDelayInfo() {From = new TimeSpan(0 ,0 ,0), To = new TimeSpan(0,15,0), AppliesTo = LK.Analyzer.DayOfWeek.Monday, Delay = 10};

		//  Assert.Single(target);
		//  Assert.Equal(expected, target.Single());
		//}

		//[Fact()]
		//public void GetTrafficDelaysRetursSingleObjectAlignedToTheTimeResolution_SingleTimeBin2() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(0, 12, 0), new TimeSpan(0, 13, 0), LK.Analyzer.DayOfWeek.Friday, 10);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected = new TrafficDelayInfo() { From = new TimeSpan(0, 0, 0), To = new TimeSpan(0, 15, 0), AppliesTo = LK.Analyzer.DayOfWeek.Friday, Delay = 10 };

		//  Assert.Single(target);
		//  Assert.Equal(expected, target.Single());
		//}

		//[Fact()]
		//public void GetTrafficDelaysRetursSingleObjectAlignedToTheTimeResolution_MoreTimeBins() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(5, 53, 0), LK.Analyzer.DayOfWeek.Monday, 10);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(6, 0, 0), AppliesTo = LK.Analyzer.DayOfWeek.Monday, Delay = 10 };

		//  Assert.Single(target);
		//  Assert.Equal(expected, target.Single());
		//}

		//[Fact()]
		//public void GetTrafficDelaysRetursSingleObjectAlignedToTheTimeResolution_MoreTimeBins2() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(5, 53, 0), LK.Analyzer.DayOfWeek.Saturday, 10);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(6, 0, 0), AppliesTo = LK.Analyzer.DayOfWeek.Saturday, Delay = 10 };

		//  Assert.Single(target);
		//  Assert.Equal(expected, target.Single());
		//}

		//[Fact()]
		//public void GetTrafficDelayReturnsTwoNonOverlapingDelaysForTestData() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(5, 53, 0), LK.Analyzer.DayOfWeek.Saturday, 10);
		//  tdm.AddDelay(new TimeSpan(16, 19, 0), new TimeSpan(23, 10, 0), LK.Analyzer.DayOfWeek.Saturday, 10);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected1 = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(6, 0, 0), AppliesTo = LK.Analyzer.DayOfWeek.Saturday, Delay = 10 };
		//  TrafficDelayInfo expected2 = new TrafficDelayInfo() { From = new TimeSpan(16, 15, 0), To = new TimeSpan(23, 15, 0), AppliesTo = LK.Analyzer.DayOfWeek.Saturday, Delay = 10 };

		//  Assert.Equal(2, target.Count());
		//  Assert.Contains(expected1, target);
		//  Assert.Contains(expected2, target);
		//}

		//[Fact()]
		//public void GetTrafficDelayReturnsJoinsTwoOverlapingDelaysForTestData() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Saturday, 10);
		//  tdm.AddDelay(new TimeSpan(10, 31, 0), new TimeSpan(11, 10, 0), LK.Analyzer.DayOfWeek.Saturday, 10);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(13, 0, 0), AppliesTo = LK.Analyzer.DayOfWeek.Saturday, Delay = 10 };

		//  Assert.Single(target);
		//  Assert.Contains(expected, target);
		//}

		//[Fact()]
		//public void GetTrafficDelayReturnsTwoOverlapingDelaysForTestData() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Saturday, 10);
		//  tdm.AddDelay(new TimeSpan(11, 31, 0), new TimeSpan(13, 10, 0), LK.Analyzer.DayOfWeek.Saturday, 15);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected1 = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(11, 30, 0), AppliesTo = LK.Analyzer.DayOfWeek.Saturday, Delay = 10 };
		//  TrafficDelayInfo expected2 = new TrafficDelayInfo() { From = new TimeSpan(11, 30, 0), To = new TimeSpan(13, 15, 0), AppliesTo = LK.Analyzer.DayOfWeek.Saturday, Delay = 15 };

		//  Assert.Equal(2, target.Count());
		//  Assert.Contains(expected1, target);
		//  Assert.Contains(expected2, target);
		//}

		//[Fact()]
		//public void GetTrafficDelayReturnsAllDaysDataForAlldays() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Any, 10);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(13, 00, 0), AppliesTo = LK.Analyzer.DayOfWeek.Any, Delay = 10 };

		//  Assert.Equal(1, target.Count());
		//  Assert.Contains(expected, target);
		//}

		//[Fact()]
		//public void GetTrafficDelayReturnsWeekendDataForWeekends() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Weekend, 10);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(13, 00, 0), AppliesTo = LK.Analyzer.DayOfWeek.Weekend, Delay = 10 };

		//  Assert.Equal(1, target.Count());
		//  Assert.Contains(expected, target);
		//}

		//[Fact()]
		//public void GetTrafficDelayReturnsWorkdaysDataForWorkdays() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Workday, 10);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(13, 00, 0), AppliesTo = LK.Analyzer.DayOfWeek.Workday, Delay = 10 };

		//  Assert.Equal(1, target.Count());
		//  Assert.Contains(expected, target);
		//}

		//[Fact()]
		//public void GetTrafficJoinsDataThroughTheWeek() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Workday, 10);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Weekend, 10);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(13, 00, 0), AppliesTo = LK.Analyzer.DayOfWeek.Any, Delay = 10 };

		//  Assert.Equal(1, target.Count());
		//  Assert.Contains(expected, target);
		//}

		//[Fact()]
		//public void GetTrafficJoinsDataThroughTheWeek2() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Saturday, 10);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Sunday, 10);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(13, 00, 0), AppliesTo = LK.Analyzer.DayOfWeek.Weekend, Delay = 10 };

		//  Assert.Equal(1, target.Count());
		//  Assert.Contains(expected, target);
		//}

		//[Fact()]
		//public void GetTrafficJoinsDataThroughTheWeek3() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Monday, 10);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Tuesday, 10);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Wednesday, 10);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Thursday, 10);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Friday, 10);


		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(13, 00, 0), AppliesTo = LK.Analyzer.DayOfWeek.Workday, Delay = 10 };

		//  Assert.Equal(1, target.Count());
		//  Assert.Contains(expected, target);
		//}

		//[Fact()]
		//public void GetTrafficReturs3NonoverlappingDelaysFor2Overlapping_OneInTheMiddle() {
		//  TrafficDelayMap tdm = new TrafficDelayMap(15);
		//  tdm.AddDelay(new TimeSpan(4, 29, 0), new TimeSpan(12, 53, 0), LK.Analyzer.DayOfWeek.Wednesday, 10);
		//  tdm.AddDelay(new TimeSpan(6, 29, 0), new TimeSpan(10, 53, 0), LK.Analyzer.DayOfWeek.Wednesday, 20);

		//  var target = tdm.GetDelays();

		//  TrafficDelayInfo expected1 = new TrafficDelayInfo() { From = new TimeSpan(4, 15, 0), To = new TimeSpan(6, 15, 0), AppliesTo = LK.Analyzer.DayOfWeek.Wednesday, Delay = 10 };
		//  TrafficDelayInfo expected2 = new TrafficDelayInfo() { From = new TimeSpan(6, 15, 0), To = new TimeSpan(11, 00, 0), AppliesTo = LK.Analyzer.DayOfWeek.Wednesday, Delay = 20 };
		//  TrafficDelayInfo expected3 = new TrafficDelayInfo() { From = new TimeSpan(11, 00, 0), To = new TimeSpan(13, 00, 0), AppliesTo = LK.Analyzer.DayOfWeek.Wednesday, Delay = 10 };

		//  Assert.Equal(3, target.Count());
		//  Assert.Contains(expected1, target);
		//  Assert.Contains(expected2, target);
		//  Assert.Contains(expected3, target);
		//}
	}
}
