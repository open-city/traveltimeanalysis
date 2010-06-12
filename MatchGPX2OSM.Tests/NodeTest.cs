using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GeoUtils.Geometry;
using LK.MatchGPX2OSM;

namespace MatchGPX2OSM.Tests {
	public class NodeTest {
		[Fact()]
		public void NodeConstructorParameterlessInitializesProperties() {
			Node target = new Node();

			Assert.NotNull(target.Connections);
		}

		[Fact()]
		public void NodeConstructorSetsPositionAndInitializeProperties() {
			PointGeo position = new PointGeo(12.8, 45.9);

			Node target = new Node(position);

			Assert.Equal(position, target.Position);
			Assert.NotNull(target.Connections);
		}

		[Fact()]
		public void NodeAddConnectionAddsConnection() {
			Node connectionEnd = new Node();
			Node target = new Node();

			Connection addedConnection = new Connection(target, connectionEnd);

			Assert.Equal(1, target.Connections.Count());
			Assert.Equal(addedConnection, target.Connections.Single());
		}

		[Fact()]
		public void NodeRemoveConnectionRemovesConnection() {
			Node connectionEnd = new Node();
			Node target = new Node();
			Connection firstConnection = new Connection(target, new Node());
			Connection toRemove = new Connection(target, connectionEnd);

			bool result = target.RemoveConnection(toRemove);

			Assert.True(result);
			Assert.Equal(1, target.Connections.Count());
			Assert.Equal(firstConnection, target.Connections.Single());
		}
	}
}
