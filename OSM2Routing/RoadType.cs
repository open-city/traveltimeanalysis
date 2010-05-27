using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.OSMUtils.OSMDatabase;

namespace LK.OSM2Routing {
	public class RoadType {
		/// <summary>
		/// Gets or sets the name of this RoadType
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets typical speed for this RoadType
		/// </summary>
		public double Speed { get; set; }

		/// <summary>
		/// Gets the list of required tags
		/// </summary>
		public List<OSMTag> RequiredTags { get; protected set; }

		/// <summary>
		/// Creates a new instance of RoadType object
		/// </summary>
		public RoadType() {
			RequiredTags = new List<OSMTag>();
		}

		/// <summary>
		/// Tests whether specific way matched to this road type
		/// </summary>
		/// <param name="toMatch">The OSMWay to be tested</param>
		/// <returns>true if the way matched to this road type, otherwise returns false</returns>
		public bool Match(OSMWay toMatch) {
			foreach (OSMTag required in RequiredTags) {
				if (toMatch.Tags.ContainsTag(required.Key)) {
					if (required.Value != "*" && toMatch.Tags[required.Key].Value != required.Value) {
						return false;
					}
				}
				else {
					return false;
				}
			}

			return true;
		}
	}
}
