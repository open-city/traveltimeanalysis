using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.GeoUtils.Geometry;
using OSMUtils.OSMDatabase;

namespace LK.GeoUtils {
	public static class Extensions {
		public static void AddWays(this Polygon<OSMNode> polygon, IList<OSMWay> ways, OSMDB db) {
			if (ways.Count == 1) {
				// Check if the created polygon is closed
				if (ways[0].Nodes.Count > 0 && ways[0].Nodes.First() != ways[0].Nodes.Last()) {
					throw new ArgumentException("Ways does not form a closed polygon");
				}

				for (int i = 0; i < ways[0].Nodes.Count - 1; i++) {
					polygon.AddVertex(db.Nodes[ways[0].Nodes[i]]);
				}
			}
			else {
				int lastVertexID = 0;

				if (ways[0].Nodes.First() == ways.Last().Nodes.First() || ways[0].Nodes.Last() == ways.Last().Nodes.First()) {
					lastVertexID = ways.Last().Nodes.First();
				}
				else {
					lastVertexID = ways.Last().Nodes.Last();
				}
				//// Check orientation of the first way
				//if (ways[0].Nodes.First() == ways[1].Nodes.First() || ways[0].Nodes.First() == ways[1].Nodes.First()) {
				//  for (int ii = ways[0].; ii < verticesToAdd.Count - 1; ii++) {
				//    AddVertex(verticesToAdd[ii]);
				//  }
				//}

				for (int i = 0; i < ways.Count; i++) {
					List<int> verticesToAdd = new List<int>();

					// Checks the way orienatation and picks nodes in correct order
					if (lastVertexID == ways[i].Nodes[0]) {
						verticesToAdd.AddRange(ways[i].Nodes);
					}
					else if (lastVertexID == ways[i].Nodes.Last()) {
						verticesToAdd.AddRange(ways[i].Nodes.Reverse());
					}
					else {
						throw new ArgumentException("Can not create polygon, ways aren't connected");
					}


					for (int ii = 0; ii < verticesToAdd.Count - 1; ii++) {
						polygon.AddVertex(db.Nodes[verticesToAdd[ii]]);
					}

					lastVertexID = verticesToAdd.Last();
				}

				// Check if the created polygon is closed
				if (polygon.VerticesCount > 0 && polygon.Vertices.First() != db.Nodes[lastVertexID]) {
					throw new ArgumentException("Ways does not form a closed polygon");
				}
			}
		}
	}
}
