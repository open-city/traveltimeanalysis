using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.MatchGPX2OSM {
	public class PathSegment {
		public Node From { get; set; }
		public Node To { get; set; }
		public ConnectionGeometry Road { get; set; }
	}
}
