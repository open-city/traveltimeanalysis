using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GeoUtils.Geometry;
using LK.MatchGPX2OSM;

namespace MatchGPX2OSM.Tests {
	public class ConnectionTest {
		[Fact()]
		public void ConnectionConstructorSetFromAndToProperties() {
			Node from = new Node();
			Node to = new Node();

			Connection target = new Connection(from, to);

			Assert.Equal(from, target.From);
			Assert.Equal(to, target.To);
		}

		[Fact()]
		public void ConnectionFromPropertyReturnsCorrectNode() {
			Node from = new Node();
			Node to = new Node();

			Connection target = new Connection(from, to);

			Node newFrom = new Node();
			target.From = newFrom;

			Assert.Equal(newFrom, target.From);
		}

		[Fact()]
		public void ConnectionToPropertyReturnsCorrectNode() {
			Node from = new Node();
			Node to = new Node();

			Connection target = new Connection(from, to);

			Node newTo = new Node();
			target.To = newTo;

			Assert.Equal(newTo, target.To);
		}
	}
}
