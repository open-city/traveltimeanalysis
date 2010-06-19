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

			Assert.Equal(position, target.MapPoint);
			Assert.NotNull(target.Connections);
		}
	}
}
