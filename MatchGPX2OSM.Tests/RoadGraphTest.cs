using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.MatchGPX2OSM;

namespace MatchGPX2OSM.Tests {
	public class RoadGraphTest {
		[Fact()]
		public void RoadGraphConstructorInitializesProperties() {
			RoadGraph target = new RoadGraph();

			Assert.NotNull(target.Nodes);
			Assert.NotNull(target.Connections);
		}
	}
}
