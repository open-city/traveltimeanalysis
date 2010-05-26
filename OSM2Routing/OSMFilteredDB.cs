using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.OSMUtils.OSMDatabase;
using LK.OSMUtils.OSMDataSource;
using System.IO;

namespace LK.OSM2Routing {
	/// <summary>
	/// Represents a OSMDB, that's can load only ways matching specific RoadTypes
	/// </summary>
	public class OSMFilteredDB : OSMDB  {
		Dictionary<int, int> _usedNodes;
		IEnumerable<RoadType> _acceptedRoads;

		/// <summary>
		/// Creates a new instance of the OSMFIlteredDB
		/// </summary>
		public OSMFilteredDB()
			: base() {
				_usedNodes = new Dictionary<int, int>();
		}

		/// <summary>
		/// Load OSM file, and filters out ways that do not match specific road types
		/// </summary>
		/// <param name="acceptedRoads">Accepted road types</param>
		/// <param name="path">Path to the OSM file</param>
		public void Load(IEnumerable<RoadType> acceptedRoads, string path) {
			using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
				Load(acceptedRoads, fs);
			}
		}

		/// <summary>
		/// Load OSM file, and filters out ways that do not match specific road types
		/// </summary>
		/// <param name="acceptedRoads">Accepted road types</param>
		/// <param name="input">Stream with OSM file</param>
		public void Load(IEnumerable<RoadType> acceptedRoads, Stream input) {
			_acceptedRoads = acceptedRoads;

			OSMXmlDataReader reader = new OSMXmlDataReader();

			reader.WayRead += WayRead;
			reader.Read(input);

			reader.WayRead -= WayRead;
			reader.NodeRead += NodeRead;
			input.Seek(0, 0);
			reader.Read(input);
		}

		/// <summary>
		/// Callback function for OSMXmlDataReader, checks whether the way matches desired RoadTypes and adds the matched road into DB
		/// </summary>
		/// <param name="way">The way read form the OSM file</param>
		void WayRead(OSMWay way) {
			foreach (RoadType road in _acceptedRoads) {
				if (road.Match(way)) {
					ExtractUsedNodes(way);
					Ways.Add(way);
				}
			}
		}

		/// <summary>
		/// Callback function for OSMXmlDataReader, checks whether node is used and adds the used node into DB
		/// </summary>
		/// <param name="node">The node read form the OSM file</param>
		void NodeRead(OSMNode node) {
			if (_usedNodes.ContainsKey(node.ID)) {
				Nodes.Add(node);
			}
		}

		/// <summary>
		/// Exctract nodes from the way and adds them into UsedNodes list
		/// </summary>
		/// <param name="way"></param>
		void ExtractUsedNodes(OSMWay way) {
			foreach (int nodeID in way.Nodes) {
				if (_usedNodes.ContainsKey(nodeID) == false) {
					_usedNodes.Add(nodeID, nodeID);
				}
			}
		}
	}
}
