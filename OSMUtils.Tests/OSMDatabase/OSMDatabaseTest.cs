using LK.OSMUtils.OSMDatabase;
using Xunit;
using System;
using System.IO;

namespace OSMUtils.Tests
{
    
    
    /// <summary>
    ///This is a test class for OSMDatabaseTest and is intended
    ///to contain all OSMDatabaseTest Unit Tests
    ///</summary>
	
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

			Assert.Equal(408,  target.Nodes.Count);
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
