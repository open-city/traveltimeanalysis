using LK.OSMUtils.OSMDataSource;
using Xunit;
using System;
using System.IO;
using System.Xml;

using LK.OSMUtils.OSMDatabase;
using System.Collections.Generic;

namespace OSMUtils.Tests {


	/// <summary>
	///This is a test class for OSMXmlDataReaderTest and is intended
	///to contain all OSMXmlDataReaderTest Unit Tests
	///</summary>
	
	public class OSMXmlDataReaderTest {
		List<OSMNode> _readNodes;
		List<OSMWay> _readWays;
		List<OSMRelation> _readRelations;

		public OSMXmlDataReaderTest() {
			_readNodes = new List<OSMNode>();
			_readWays = new List<OSMWay>();
			_readRelations = new List<OSMRelation>();
		}

		[Fact()]
		public void OSMXmlDataReaderReadThrowsExceptionIfFileDoesnotExist() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			Assert.Throws<FileNotFoundException>(delegate {target.Read("non-existing-file.osm");});
		}

		[Fact()]
		public void OSMXmlDataReaderReadThrowsExceptionReadingInvalidRootElement() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			Assert.Throws<XmlException>(delegate { target.Read(new MemoryStream(OSMUtils.Tests.TestData.osm_invalid_root_element));});
		}

		[Fact()]
		public void OSMXmlDataReaderReadSkipsUnknownElements() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			target.Read(new MemoryStream(OSMUtils.Tests.TestData.osm_unknown_inner_element));
		}

		[Fact()]
		public void OSMXmlDataReaderCanReadSimpleNode() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			target.NodeRead += new OSMNodeReadHandler(ProcessNode);

			// <node id="1254" lat="50.4" lon="16.2" />
            target.Read(new MemoryStream(OSMUtils.Tests.TestData.osm_simple_node));

			Assert.Equal(1, _readNodes.Count);

			OSMNode readNode = _readNodes[0];
			Assert.Equal(1254, readNode.ID);
			Assert.Equal(50.4, readNode.Latitude);
			Assert.Equal(16.2, readNode.Longitude);
		}

		[Fact()]
		public void OSMXmlDataReaderCanReadNodeWithTag() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			target.NodeRead += new OSMNodeReadHandler(ProcessNode);

			//  <node id="1254" lat="50.4" lon="16.2">
			//  	<tag k="name" v="test" />
			//	</node>
            target.Read(new MemoryStream(OSMUtils.Tests.TestData.osm_node_with_tag));

			OSMNode readNode = _readNodes[0];
			Assert.Equal(1, readNode.Tags.Count);
			Assert.True(readNode.Tags.ContainsTag("name"));
			Assert.Equal("test", readNode.Tags["name"].Value);
		}

		public void OSMXmlDataReaderCanReadNodeWithTags() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			target.NodeRead += new OSMNodeReadHandler(ProcessNode);

			//  <node id="1254" lat="50.4" lon="16.2">
			//  	<tag k="name" v="test" />
			//  	<tag k="name-2" v="test-2" />
			//	</node>
            target.Read(new MemoryStream(OSMUtils.Tests.TestData.osm_node_with_tags));

			OSMNode readNode = _readNodes[0];
			Assert.Equal(2, readNode.Tags.Count);

			Assert.True(readNode.Tags.ContainsTag("name"));
			Assert.Equal("test", readNode.Tags["name"].Value);

			Assert.True(readNode.Tags.ContainsTag("name-2"));
			Assert.Equal("test-2", readNode.Tags["name-2"].Value);
		}

		[Fact()]
		public void OSMXmlDataReaderCanReadNodeWithTagAndUnknownElement() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			target.NodeRead += new OSMNodeReadHandler(ProcessNode);

			//	<node id="1254" lat="50.4" lon="16.2">
			//  	<tag k="name" v="test" />
			// 	  <unknown-element parameter="aaa" />
			//	</node>
            target.Read(new MemoryStream(OSMUtils.Tests.TestData.osm_node_with_tag_and_unknown_element));

			OSMNode readNode = _readNodes[0];
			Assert.Equal(1, readNode.Tags.Count);
			Assert.True(readNode.Tags.ContainsTag("name"));
			Assert.Equal("test", readNode.Tags["name"].Value);
		}

		[Fact()]
		public void OSMXmlDataReaderCanReadSimpleWay() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			target.WayRead += new OSMWayReadHandler(ProcessWay);

			//  <way id="1254">
			//  	<nd ref="3" />
			//  	<nd ref="4" />
			//	</way>
            target.Read(new MemoryStream(OSMUtils.Tests.TestData.osm_simple_way));

			Assert.Equal(1, _readWays.Count);

			OSMWay readWay = _readWays[0];

			Assert.Equal(1254, readWay.ID);
			Assert.Equal(2, readWay.Nodes.Count);

			Assert.Equal(3, readWay.Nodes[0]);
			Assert.Equal(4, readWay.Nodes[1]);
		}

		[Fact()]
		public void OSMXmlDataReaderCanReadWayWithTags() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			target.WayRead += new OSMWayReadHandler(ProcessWay);

			//  <way id="1254">
			//  	<nd ref="3" />
			//  	<nd ref="4" />
			//  	<tag k="name" v="test" />
			//  	<tag k="name-2" v="test-2" />
			//	</way>
            target.Read(new MemoryStream(OSMUtils.Tests.TestData.osm_way_with_tags));

			Assert.Equal(1, _readWays.Count);

			OSMWay readWay = _readWays[0];

			Assert.Equal(2, readWay.Nodes.Count);
			Assert.Equal(2, readWay.Tags.Count);
		}

		[Fact()]
		public void OSMXmlDataReaderCanReadSimpleRelation() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			target.RelationRead += new OSMRelationReadHandler(ProcessRelation);

			//  <relation id="1254">
			//  	<member ref="3" type="node" role="test" />
			//	</relation>
            target.Read(new MemoryStream(OSMUtils.Tests.TestData.osm_simple_relation));

			Assert.Equal(1, _readRelations.Count);
			Assert.Equal(1254, _readRelations[0].ID);

			Assert.Equal(1, _readRelations[0].Members.Count);
		}

		[Fact()]
		public void OSMXmlDataReaderCanReadRelationWithVariousMemberTypes() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			target.RelationRead += new OSMRelationReadHandler(ProcessRelation);

			//  <relation id="1254">
			//		<member ref="3" type="node" role="test" />
			//		<member ref="4" type="way" role="test" />
			//		<member ref="5" type="relation" role="test" />
			//	</relation>
			target.Read(new MemoryStream(OSMUtils.Tests.TestData.osm_relation));

			Assert.Equal(1, _readRelations.Count);

			Assert.Equal(3, _readRelations[0].Members.Count);

			Assert.Equal(3, _readRelations[0].Members[0].Reference);
			Assert.Equal("test", _readRelations[0].Members[0].Role);
			Assert.Equal(OSMRelationMemberType.node, _readRelations[0].Members[0].Type);

			Assert.Equal(4, _readRelations[0].Members[1].Reference);
			Assert.Equal("test", _readRelations[0].Members[1].Role);
			Assert.Equal(OSMRelationMemberType.way, _readRelations[0].Members[1].Type);

			Assert.Equal(5, _readRelations[0].Members[2].Reference);
			Assert.Equal("test", _readRelations[0].Members[2].Role);
			Assert.Equal(OSMRelationMemberType.relation, _readRelations[0].Members[2].Type);
		}

		[Fact()]
		public void OSMXmlDataReaderCanReadRelationWithTags() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			target.RelationRead += new OSMRelationReadHandler(ProcessRelation);

			//  <relation id="1254">
			//		<member ref="3" type="node" role="test" />
			//		<tag k="name" v="test-relation" />
			//	</relation>
            target.Read(new MemoryStream(OSMUtils.Tests.TestData.osm_relation_with_tag));

			Assert.Equal(1, _readRelations.Count);
			Assert.Equal(1, _readRelations[0].Tags.Count);
		}

		[Fact()]
		public void OSMXmlDataReaderCanReadRealOSMFile() {
			OSMXmlDataReader target = new OSMXmlDataReader();
			target.NodeRead += new OSMNodeReadHandler(ProcessNode);
			target.WayRead += new OSMWayReadHandler(ProcessWay);
			target.RelationRead += new OSMRelationReadHandler(ProcessRelation);

            target.Read(new MemoryStream(OSMUtils.Tests.TestData.real_osm_file));

			Assert.Equal(408, _readNodes.Count);
			Assert.Equal(22, _readWays.Count);
			Assert.Equal(2, _readRelations.Count);
		}

		/// <summary>
		/// Saves the read OSMNode to the internal storage
		/// </summary>
		/// <param name="node">The OSMNode read</param>
		public void ProcessNode(OSMNode node) {
			_readNodes.Add(node);
		}

		/// <summary>
		/// Saves the read OSMWay to the internal storage
		/// </summary>
		/// <param name="node">The OSMWay read</param>
		public void ProcessWay(OSMWay way) {
			_readWays.Add(way);
		}

		/// <summary>
		/// Saves the read OSMRelation to the internal storage
		/// </summary>
		/// <param name="node">The OSMRelation read</param>
		public void ProcessRelation(OSMRelation relation) {
			_readRelations.Add(relation);
		}
	}
}
