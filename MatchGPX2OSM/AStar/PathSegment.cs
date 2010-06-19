using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.MatchGPX2OSM {
	/// <summary>
	/// Represent part of the path beteen two points
	/// </summary>
	public class PathSegment {
		public Node From { get; set; }
		public Node To { get; set; }
		public ConnectionGeometry Road { get; set; }
	}
}
