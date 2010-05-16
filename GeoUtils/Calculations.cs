using System;
using System.Collections.Generic;
using System.Text;

namespace OSMUtils {
	//struct PointGeo {
	//  public double Lat;
	//  public double Lon;
	//}

	public static class Calculations {
	//  static double CalculateDistance(int p1Id, int p2Id, IOsmDatabase db) {
	//    const double earthRadius = 6371010.0;

	//    PointGeo pt1 = new PointGeo() {Lat = db.GetNode(p1Id).Latitude, Lon = db.GetNode(p1Id).Longitude};
	//    PointGeo pt2 = new PointGeo() {Lat = db.GetNode(p2Id).Latitude, Lon = db.GetNode(p2Id).Longitude};

	//    double dLat = toRadians(pt2.Lat - pt1.Lat);
	//    double dLon = toRadians(pt2.Lon - pt1.Lon);

	//    double a = Math.Sin(dLat / 2.0) * Math.Sin(dLat / 2.0) + Math.Cos(toRadians(pt1.Lat)) * Math.Cos(toRadians(pt2.Lat)) * Math.Sin(dLon / 2.0) * Math.Sin(dLon / 2.0);
	//    double dAngle = 2 * Math.Asin(Math.Sqrt(a));

	//    return dAngle * earthRadius;
	//  }

	//  static double toRadians(double angle) {
	//    return angle * Math.PI / 180;
	//  }

	//  public static List<int> DPReduction(OsmWay w, IOsmDatabase db) {
	//    bool[] vertexUsed = new bool[w.NodesCount];
	//    vertexUsed[0] = true;
	//    vertexUsed[vertexUsed.Length - 1] = true;

	//    Reduce(w, ref vertexUsed, 0, vertexUsed.Length - 1, db);

	//    List<int> toRemove = new List<int>();
	//    for (int i = 0; i < vertexUsed.Length; i++) {
	//      if (vertexUsed[i] == false) {
	//        toRemove.Add(w.Nodes[i]);
	//      }
	//    }
			
	//    //toRemove.ForEach(node => w.Nodes.Remove(node));

	//    return toRemove;
	//  }

	//  static void Reduce(OsmWay w, ref bool[] vertexUsed, int a, int b, IOsmDatabase db) {
	//    double tolerance = 5.0;

	//    if (b <= a + 1)
	//      return;

	//    double maxDistance = 0;
	//    int maxDistanceIndex = -1;

	//    double distAB = CalculateDistance(w.Nodes[a], w.Nodes[b], db);
	//    for (int i = a + 1; i < b; i++) {

	//      double distAP = CalculateDistance(w.Nodes[a], w.Nodes[i], db);
	//      double distBP = CalculateDistance(w.Nodes[b], w.Nodes[i], db);

	//      double cosA = (distBP * distBP - distAP * distAP - distAB * distAB) / (-2 * distAB * distAP);
	//      double sinA = Math.Sqrt(1 - cosA * cosA);
	//      double distance = sinA * distAP;


	//      if (distance > maxDistance) {
	//        maxDistance = distance;
	//        maxDistanceIndex = i;
	//      }
	//    }

	//    if (maxDistance > tolerance) {
	//      vertexUsed[maxDistanceIndex] = true;
	//      Reduce(w, ref vertexUsed, a, maxDistanceIndex, db);
	//      Reduce(w, ref vertexUsed, maxDistanceIndex, b, db);
	//    }
	//  }
	}
}
