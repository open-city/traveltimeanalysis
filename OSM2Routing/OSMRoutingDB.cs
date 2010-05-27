using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.OSMUtils.OSMDatabase;
using LK.OSMUtils.OSMDataSource;
using System.IO;

namespace LK.OSM2Routing {
	/// <summary>
	/// Represents a OSMDB that can save it's content id routing-friendly form
	/// </summary>
	public class OSMRoutingDB : OSMDB  {
		Dictionary<int, List<int>> _usedNodes;
		/// <summary>
		/// Gets the used nodes and ways that contains them
		/// </summary>
		public Dictionary<int, List<int>> UsedNodes {
			get {
				return _usedNodes;
			}
		}

		IEnumerable<RoadType> _acceptedRoads;

		/// <summary>
		/// Creates a new instance of the OSMFIlteredDB
		/// </summary>
		public OSMRoutingDB()
			: base() {
				_usedNodes = new Dictionary<int, List<int>>();
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
					Ways.Add(new OSMRoad(way, road));
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
					_usedNodes.Add(nodeID, new List<int>());
				}

				_usedNodes[nodeID].Add(way.ID);
			}
		}

		/// <summary>
		/// Splits ways at road crossings, check for oneway roads and save results in OSMDB
		/// </summary>
		/// <returns>OSMDB object with road segments and used nodes</returns>
		public OSMDB BuildRoutableOSM() {
			OSMDB result = new OSMDB();
			int counter = -1;

			foreach (OSMRoad route in Ways) {
				OSMWay segment = new OSMWay(counter--);
				OSMTag wayIDTag = new OSMTag("way-id", route.ID.ToString());
				OSMTag speedTag = new OSMTag("speed", route.RoadType.Speed.ToString());

				string wayAccessibility = route.IsAccessible() ? "yes" : "no";
				OSMTag wayAccessibilityTag = new OSMTag("accessible", wayAccessibility);

				string wayAccessibilityReverse = route.IsAccessibleReverse() ? "yes" : "no";
				OSMTag wayAccessibilityReverseTag = new OSMTag("accessible-reverse", wayAccessibilityReverse);

				for (int i = 0; i < route.Nodes.Count; i++) {
					segment.Nodes.Add(route.Nodes[i]);

					if ((UsedNodes[route.Nodes[i]].Count > 1) && (i > 0) && (i < (route.Nodes.Count -1))) {
						segment.Tags.Add(wayIDTag);
						segment.Tags.Add(speedTag);
						segment.Tags.Add(wayAccessibilityTag);
						segment.Tags.Add(wayAccessibilityReverseTag);

						result.Ways.Add(segment);

						segment = new OSMWay(counter--);
						segment.Nodes.Add(route.Nodes[i]);
					}
				}

				segment.Tags.Add(wayIDTag);
				segment.Tags.Add(speedTag);
				segment.Tags.Add(wayAccessibilityTag);
				segment.Tags.Add(wayAccessibilityReverseTag);
				result.Ways.Add(segment);
			}

			foreach (OSMNode node in Nodes) {
				result.Nodes.Add(node);
			}

			return result;
		}
	}
}
