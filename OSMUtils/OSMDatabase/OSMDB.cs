using System;
using System.Collections.Generic;
using System.Text;

using LK.OSMUtils.OSMDataSource;
using System.IO;

namespace LK.OSMUtils.OSMDatabase {
	/// <summary>
	/// Represents an OSM database
	/// </summary>
	public class OSMDB {
		/// <summary>
		/// Creates a new OSMDB object
		/// </summary>
		public OSMDB() {
			_nodes = new OSMObjectCollection<OSMNode>();
			_ways = new OSMObjectCollection<OSMWay>();
			_relations = new OSMObjectCollection<OSMRelation>();
		}

		/// <summary>
		/// Loads OSM entities from the specific OSM file
		/// </summary>
		/// <param name="filename">Path to the OSM file</param>
		public virtual void Load(string filename) {
			using (FileStream fs = new FileStream(filename, FileMode.Open)) {
				this.Load(fs);
			}
		}

		/// <summary>
		/// Loads OSM entities from the specific Stream
		/// </summary>
		/// <param name="stream">Stream with the OSM file</param>
		public virtual void Load(Stream stream) {
			OSMXmlDataReader xmlReader = new OSMXmlDataReader();
			xmlReader.NodeRead += new OSMNodeReadHandler(node => _nodes.Add(node));
			xmlReader.WayRead += new OSMWayReadHandler(way => _ways.Add(way));
			xmlReader.RelationRead += new OSMRelationReadHandler(relation => _relations.Add(relation));

			xmlReader.Read(stream);
		}

		/// <summary>
		/// Saves OSM Database to the specific OSM file
		/// </summary>
		/// <param name="filename">Path to the OSM file</param>
		public void Save(string filename) {
			using (FileStream fs = new FileStream(filename, FileMode.Create)) {
				this.Save(fs);
			}
		}

		/// <summary>
		/// Saves OSM Database to the specific Stream
		/// </summary>
		/// <param name="stream">Stream to to save OSM database</param>
		public void Save(Stream stream) {
			using (OSMXmlDataWriter writer = new OSMXmlDataWriter(stream)) {

				foreach (var node in _nodes) {
					writer.WriteNode(node);
				}

				foreach (var way in _ways) {
					writer.WriteWay(way);
				}

				foreach (var relation in _relations) {
					writer.WriteRelation(relation);
				}

				writer.Close();
			}
		}

		protected OSMObjectCollection<OSMNode> _nodes;
		/// <summary>
		/// Gets the collection of OSMNodes
		/// </summary>
		public OSMObjectCollection<OSMNode> Nodes {
			get {
				return _nodes;
			}
		}

		OSMObjectCollection<OSMWay> _ways;
		/// <summary>
		/// Gets the collection of OSMWays
		/// </summary>
		public OSMObjectCollection<OSMWay> Ways {
			get {
				return _ways;
			}
		}


		OSMObjectCollection<OSMRelation> _relations;
		/// <summary>
		/// Get the collection of OSMRelations
		/// </summary>
		public OSMObjectCollection<OSMRelation> Relations {
			get {
				return _relations;
			}
		}

	}
}
