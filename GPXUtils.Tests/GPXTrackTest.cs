using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GPXUtils;

namespace GPXUtils.Tests {
	public class GPXTrackTest {
		[Fact()]
		public void GPXTrackConstructorCreatesEmptyTrack() {
			GPXTrack target = new GPXTrack();

			Assert.Equal(0, target.Segments.Count);
		}

		[Fact()]
		public void GPXTrackConstructorCreatesEmptyTrackWithName() {
			string name = "Test";

			GPXTrack target = new GPXTrack(name);

			Assert.Equal(0, target.Segments.Count);
			Assert.Equal(name, target.Name);
		}
	}
}
