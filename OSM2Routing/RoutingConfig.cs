using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml;

using LK.OSMUtils.OSMDatabase;

namespace LK.OSM2Routing {
	/// <summary>
	/// Represents the config file with parameters for processing OSM to routable OSM
	/// </summary>
	public class RoutingConfig {
		public List<RoadType> RoadTypes { get; protected set; }

		/// <summary>
		/// Creates a new instance of RoutingConfig
		/// </summary>
		public RoutingConfig() {
			RoadTypes = new List<RoadType>();
		}

		/// <summary>
		/// Loads config data from the file
		/// </summary>
		/// <param name="path">Path to the config file</param>
		public void Load(string path) {
			using (FileStream fs = new FileStream(path, FileMode.Open)) {
				Load(fs);
			}
		}
		
		/// <summary>
		/// Loads config data from the stream
		/// </summary>
		/// <param name="input">Input stream with config data</param>
		public void Load(Stream input) {
			XDocument doc = XDocument.Load(new StreamReader(input));
			XElement root = doc.Root;

			if (root.Name != "routng-config") {
				throw new XmlException("Wrong root element, expected <routng-config>");
			}

			foreach (var roadTypeElement in root.Elements("route-type")) {
				RoadType parsedType = new RoadType();
				parsedType.Name = roadTypeElement.Attribute("name").Value;

				foreach (var tagElement in roadTypeElement.Elements("required-tag")) {
					parsedType.RequiredTags.Add(new OSMTag(tagElement.Attribute("key").Value, tagElement.Attribute("value").Value));
				}

				RoadTypes.Add(parsedType);
			}
		}

	}
}
