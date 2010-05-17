using LK.OSMUtils.OSMDatabase;
using Xunit;
using System;

namespace OSMUtils.Tests
{
    /// <summary>
    ///This is a test class for OSMWayTest and is intended
    ///to contain all OSMWayTest Unit Tests
    ///</summary>
	
	public class OSMWayTest {
		[Fact()]
		public void OSMWayConstructorInitializesProperties() {
			int id = 1374;
			OSMWay target = new OSMWay(id);

			Assert.Equal(id, target.ID);
			Assert.NotNull(target.Nodes);
		}

		[Fact()]
		public void OSMWayIsClosedReturnsFalseForWaysWithLessThan3Nodes() {
			int id = 1374;
			OSMWay target = new OSMWay(id);
			target.Nodes.Add(1);
			target.Nodes.Add(1);

			Assert.False(target.IsClosed);
		}

		[Fact()]
		public void OSMWayIsClosedReturnsFalseForOpenedWays() {
			int id = 1374;
			OSMWay target = new OSMWay(id);
			target.Nodes.Add(1);
			target.Nodes.Add(2);
			target.Nodes.Add(3);

			Assert.False(target.IsClosed);
		}

		[Fact()]
		public void OSMWayIsClosedReturnsTrueForClosedWays() {
			int id = 1374;
			OSMWay target = new OSMWay(id);
			target.Nodes.Add(1);
			target.Nodes.Add(2);
			target.Nodes.Add(3);
			target.Nodes.Add(1);

			Assert.True(target.IsClosed);
		}
	}
}
