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

using LK.OSMUtils.OSMDataSource;
using Xunit;
using System;
using System.IO;
using System.Xml.Linq;

using LK.OSMUtils.OSMDatabase;

namespace OSMUtils.Tests {
	/// <summary>
	///This is a test class for OSMXmlDataWriterTest and is intended
	///to contain all OSMXmlDataWriterTest Unit Tests
	///</summary>

	public class OSMXmlDataWriterTest {
		[Fact()]
		public void OSMXmlDataWriterConstructorCreatesEmptyOSMFile() {
			MemoryStream ms = new MemoryStream();

			using (OSMXmlDataWriter target = new OSMXmlDataWriter(ms)) {
			}

			ms.Seek(0, 0);
			XElement osmRoot = XDocument.Load(new StreamReader(ms)).Root;

			Assert.Equal("osm", osmRoot.Name);
			Assert.False(osmRoot.HasElements);
		}

		[Fact()]
		public void OSMXmlDataWriterCloseClosesWriterAndDoesNotAllowFutherWriting() {
			MemoryStream ms = new MemoryStream();

			using (OSMXmlDataWriter target = new OSMXmlDataWriter(ms)) {
				target.Close();

				Assert.Throws<InvalidOperationException>(delegate { target.WriteNode(new OSMNode(1, 0.1, 0.2)); });
			}
		}

		[Fact()]
		public void OSMXmlDataWriterWriteOSMObjectAttributesWritesAllCommonAttributes() {
			MemoryStream ms = new MemoryStream();

			using (OSMXmlDataWriter target = new OSMXmlDataWriter(ms)) {
				OSMNode node = new OSMNode(1254, 12.4, 15.9);
				target.WriteNode(node);
			}

			ms.Seek(0, 0);
			XElement osmRoot = XDocument.Load(new StreamReader(ms)).Root;
			XElement nodeElement = osmRoot.Element("node");

			Assert.NotNull(nodeElement);
			Assert.Equal(1254, int.Parse(nodeElement.Attribute("id").Value, System.Globalization.NumberFormatInfo.InvariantInfo));
			Assert.Equal("true", nodeElement.Attribute("visible").Value);
		}

		[Fact()]
		public void OSMXmlDataWriterWriteNodeCanWriteNodeToOutput() {
			MemoryStream ms = new MemoryStream();

			using (OSMXmlDataWriter target = new OSMXmlDataWriter(ms)) {
				OSMNode node = new OSMNode(1254, 12.4, 15.9);
				target.WriteNode(node);
			}

			ms.Seek(0, 0);
			XElement osmRoot = XDocument.Load(new StreamReader(ms)).Root;
			XElement nodeElement = osmRoot.Element("node");

			Assert.NotNull(nodeElement);
			Assert.Equal(12.4, Double.Parse(nodeElement.Attribute("lat").Value, System.Globalization.NumberFormatInfo.InvariantInfo));
			Assert.Equal(15.9, Double.Parse(nodeElement.Attribute("lon").Value, System.Globalization.NumberFormatInfo.InvariantInfo));
		}

		[Fact()]
		public void OSMXmlDataWriterWriteTags() {
			MemoryStream ms = new MemoryStream();

			using (OSMXmlDataWriter target = new OSMXmlDataWriter(ms)) {
				OSMNode node = new OSMNode(1254, 12.4, 15.9);
				node.Tags.Add(new OSMTag("name", "test"));

				target.WriteNode(node);
			}

			ms.Seek(0, 0);
			XElement osmRoot = XDocument.Load(new StreamReader(ms)).Root;
			XElement tagElement = osmRoot.Element("node").Element("tag");

			Assert.NotNull(tagElement);
			Assert.Equal("name", tagElement.Attribute("k").Value);
			Assert.Equal("test", tagElement.Attribute("v").Value);
		}

		[Fact()]
		public void OSMXmlDataWriteWayCanWriteWay() {
			MemoryStream ms = new MemoryStream();

			using (OSMXmlDataWriter target = new OSMXmlDataWriter(ms)) {
				OSMWay way = new OSMWay(1232);
				way.Nodes.Add(1);

				target.WriteWay(way);
			}

			ms.Seek(0, 0);
			XElement osmRoot = XDocument.Load(new StreamReader(ms)).Root;
			XElement wayElement = osmRoot.Element("way");

			Assert.NotNull(wayElement);
			Assert.Equal(1232, int.Parse(wayElement.Attribute("id").Value, System.Globalization.NumberFormatInfo.InvariantInfo));

			Assert.Equal(1, int.Parse(wayElement.Element("nd").Attribute("ref").Value, System.Globalization.NumberFormatInfo.InvariantInfo));
		}

		[Fact()]
		public void OSMXmlDataWriteWayCanWriteWayWithTags() {
			MemoryStream ms = new MemoryStream();

			using (OSMXmlDataWriter target = new OSMXmlDataWriter(ms)) {
				OSMWay way = new OSMWay(1232);
				way.Nodes.Add(1);
				way.Tags.Add(new OSMTag("name", "test"));

				target.WriteWay(way);
			}

			ms.Seek(0, 0);
			XElement osmRoot = XDocument.Load(new StreamReader(ms)).Root;
			XElement wayElement = osmRoot.Element("way");

			Assert.NotNull(wayElement);
			Assert.Equal(1232, int.Parse(wayElement.Attribute("id").Value, System.Globalization.NumberFormatInfo.InvariantInfo));

			Assert.Equal(1, int.Parse(wayElement.Element("nd").Attribute("ref").Value, System.Globalization.NumberFormatInfo.InvariantInfo));

			Assert.Equal("name", wayElement.Element("tag").Attribute("k").Value);

		}

		[Fact()]
		public void OSMXmlDataWriteRelationCanWriteSimpleRelation() {
			MemoryStream ms = new MemoryStream();

			using (OSMXmlDataWriter target = new OSMXmlDataWriter(ms)) {
				OSMRelation relation = new OSMRelation(1232);
				relation.Members.Add(new OSMRelationMember(OSMRelationMemberType.node, 12, "test"));

				target.WriteRelation(relation);
			}

			ms.Seek(0, 0);
			XElement osmRoot = XDocument.Load(new StreamReader(ms)).Root;
			XElement relationElement = osmRoot.Element("relation");

			Assert.NotNull(relationElement);
			Assert.Equal(1232, int.Parse(relationElement.Attribute("id").Value, System.Globalization.NumberFormatInfo.InvariantInfo));


			Assert.Equal(12, int.Parse(relationElement.Element("member").Attribute("ref").Value, System.Globalization.NumberFormatInfo.InvariantInfo));
			Assert.Equal("test", relationElement.Element("member").Attribute("role").Value);
			Assert.Equal(OSMRelationMemberType.node, Enum.Parse(typeof(OSMRelationMemberType), relationElement.Element("member").Attribute("type").Value));
		}

		[Fact()]
		public void OSMXmlDataWriteRelationCanWriteRelationWithTags() {
			MemoryStream ms = new MemoryStream();

			using (OSMXmlDataWriter target = new OSMXmlDataWriter(ms)) {
				OSMRelation relation = new OSMRelation(1232);
				relation.Members.Add(new OSMRelationMember(OSMRelationMemberType.node, 12, "test"));
				relation.Tags.Add(new OSMTag("name", "test"));

				target.WriteRelation(relation);
			}

			ms.Seek(0, 0);
			XElement osmRoot = XDocument.Load(new StreamReader(ms)).Root;
			XElement relationElement = osmRoot.Element("relation");

			Assert.NotNull(relationElement);
			Assert.Equal(1232, int.Parse(relationElement.Attribute("id").Value, System.Globalization.NumberFormatInfo.InvariantInfo));

			Assert.Equal("name", relationElement.Element("tag").Attribute("k").Value);
		}
	}
}
