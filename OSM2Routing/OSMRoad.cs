using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.OSMUtils.OSMDatabase;

namespace LK.OSM2Routing {
	/// <summary>
	/// Represent an OSMWay with that is used for building OSMRoutingDB
	/// </summary>
	public class OSMRoad : OSMWay {
		private const double MphConversionFactor = 1.609;
		/// <summary>
		/// Gets or sets RouteType for this way
		/// </summary>
		public RoadType RoadType { get; set; }

		/// <summary>
		/// Gets or sets typical speed for this road
		/// </summary>
		/// <remarks>Value is in kph.</remarks>
		public double Speed { get; set; }

		/// <summary>
		/// Creates a new instance of the OSMRoute
		/// </summary>
		/// <param name="id">The ID of the route</param>
		public OSMRoad(int id)
			: base(id) {
		}

		/// <summary>
		/// Creates a new instance of OSMRoute based on specific OSMWay
		/// </summary>
		/// <param name="way">The way that defines geomery and tags</param>
		/// <param name="roadType">The RoadType of this OSMRoute</param>
		public OSMRoad(OSMWay way, RoadType roadType) : base(way.ID) {
			_tags = way.Tags;
			_nodes = new List<int>(way.Nodes);

			RoadType = roadType;

			Speed = RoadType.Speed;

			if (Tags.ContainsTag("maxspeed")) {
				string[] chunks = Tags["maxspeed"].Value.Split(' ');
				double numericSpeed = 0;
				if (double.TryParse(chunks[0], out numericSpeed)) {
					if (chunks.Length == 2 && chunks[1] == "mph") {
						Speed = numericSpeed * MphConversionFactor;
					}
					else {
						Speed = numericSpeed;
					}
				}
			}
		}

		/// <summary>
		/// Returns accessibility of the road in backwawrd direction
		/// </summary>
		/// <returns>true if the road is accessible in backward direction, otherwise returns false</returns>
		public bool IsAccessibleReverse() {
			if (Tags.ContainsTag("oneway") == false) {
				return !RoadType.Oneway;
			}
			else {
				if (Tags["oneway"].Value == "yes" || Tags["oneway"].Value == "1" || Tags["oneway"].Value == "true") {
					return false;
				}
				else {
					return true;
				}
			}
		}

		/// <summary>
		/// Returns accessibility of the road
		/// </summary>
		/// <returns>true if the road is accessible in forward direction, otherwise returns false</returns>
		public bool IsAccessible() {
			if (Tags.ContainsTag("oneway") == false) {
				return true;
			}
			else {
				if (Tags["oneway"].Value == "-1" || Tags["oneway"].Value == "reverse") {
					return false;
				}
				else {
					return true;
				}
			}
		}

	}
}
