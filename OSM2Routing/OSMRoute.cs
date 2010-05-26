using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.OSMUtils.OSMDatabase;

namespace LK.OSM2Routing {
	/// <summary>
	/// Extends OSMWay with RoadType property
	/// </summary>
	public class OSMRoute : OSMWay {
		/// <summary>
		/// Gets or sets RouteType for this way
		/// </summary>
		public RoadType RoadType { get; set; }

		/// <summary>
		/// Creates a new instance of the OSMRoute
		/// </summary>
		/// <param name="id">The ID of the route</param>
		public OSMRoute(int id)
			: base(id) {
		}

		/// <summary>
		/// Creates a new instance of OSMRoute based on specific OSMWay
		/// </summary>
		/// <param name="way">The way that defines geomery and tags</param>
		/// <param name="roadType">The RoadType of this OSMRoute</param>
		public OSMRoute(OSMWay way, RoadType roadType) : base(way.ID) {
			_tags = way.Tags;
			_nodes = new List<int>(way.Nodes);

			RoadType = roadType;
		}
	}
}
