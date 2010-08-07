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
using System.IO;

namespace OSMUtils.Tests {
	public class OSMDatabaseTest {
		[Fact()]
		public void OSMDatabaseConstructorInitializesInternalFields() {
			OSMDB target = new OSMDB();

			Assert.Equal(0, target.Nodes.Count);
			Assert.Equal(0, target.Ways.Count);
			Assert.Equal(0, target.Relations.Count);
		}

		[Fact()]
		public void OSMDatabaseNodesAcceptsAndReturnsNodes() {
			OSMDB target = new OSMDB();
			OSMNode node = new OSMNode(1254, 1.0, 2.0);

			target.Nodes.Add(node);

			Assert.Same(node, target.Nodes[node.ID]);
		}

		[Fact()]
		public void OSMDatabaseWaysAcceptsAndReturnsWays() {
			OSMDB target = new OSMDB();
			OSMWay way = new OSMWay(1354);

			target.Ways.Add(way);

			Assert.Same(way, target.Ways[way.ID]);
		}

		[Fact()]
		public void OSMDatabaseNodesAcceptsAndReturnsRelations() {
			OSMDB target = new OSMDB();
			OSMRelation relation = new OSMRelation(1454);

			target.Relations.Add(relation);

			Assert.Same(relation, target.Relations[relation.ID]);
		}

		[Fact()]
		public void OSMDatabaseLoadCanLoadDataFromOSMFile() {
			OSMDB target = new OSMDB();
			target.Load(new MemoryStream(OSMUtils.Tests.TestData.real_osm_file));

			Assert.Equal(408, target.Nodes.Count);
			Assert.Equal(22, target.Ways.Count);
			Assert.Equal(2, target.Relations.Count);
		}

		[Fact()]
		public void OSMDatabaseSaveCanSaveDataToOSMFile() {
			OSMDB target = new OSMDB();

			target.Load(new MemoryStream(OSMUtils.Tests.TestData.real_osm_file));

			MemoryStream writtenDb = new MemoryStream();
			target.Save(writtenDb);

			writtenDb.Seek(0, 0);
			OSMDB readDB = new OSMDB();
			readDB.Load(writtenDb);

			Assert.Equal(408, readDB.Nodes.Count);
			Assert.Equal(22, readDB.Ways.Count);
			Assert.Equal(2, readDB.Relations.Count);
		}
	}
}
