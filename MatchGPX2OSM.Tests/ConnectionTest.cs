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
