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
		public void OSMWayConstructorInitializesID() {
			int id = 1374;
			OSMWay target = new OSMWay(id);

			Assert.Equal(id, target.ID);
			Assert.NotNull(target.Nodes);
		}

		[Fact()]
		public void OSMWayConstructorInitializesIDAndNodes() {
			int id = 1374;
			int[] nodes = new int[] { 1, 2, 3 };
			OSMWay target = new OSMWay(id, nodes);

			Assert.Equal(id, target.ID);
			Assert.Equal(nodes[0], target.Nodes[0]);
			Assert.Equal(nodes[1], target.Nodes[1]);
			Assert.Equal(nodes[2], target.Nodes[2]);
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
