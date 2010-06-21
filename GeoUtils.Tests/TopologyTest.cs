using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GeoUtils;
using LK.GeoUtils.Geometry;

namespace GeoUtils.Tests {
	public class TopologyTest {
		[Fact()]
		public void TopologyProjectPointReturnsSegmentOrginForPointsOutsideSegment() {
			PointGeo orgin = new PointGeo(5, 10);
			PointGeo end = new PointGeo(5, 15);
			Segment<IPointGeo> segment = new Segment<IPointGeo>(orgin, end);

			PointGeo testPoint1 = new PointGeo(5, 8);
			PointGeo testPoint2 = new PointGeo(6, 10);
			PointGeo testPoint3 = new PointGeo(4, 10);

			Assert.Equal(orgin, Topology.ProjectPoint(testPoint1, segment));
			Assert.Equal(orgin, Topology.ProjectPoint(testPoint2, segment));
			Assert.Equal(orgin, Topology.ProjectPoint(testPoint3, segment));
		}

		[Fact()]
		public void TopologyProjectPointReturnsSegmentOrginForPointsOutsideSegment2() {
			PointGeo orgin = new PointGeo(10, 5);
			PointGeo end = new PointGeo(15, 5);
			Segment<IPointGeo> segment = new Segment<IPointGeo>(orgin, end);

			PointGeo testPoint1 = new PointGeo(8, 5);
			PointGeo testPoint2 = new PointGeo(10, 6);
			PointGeo testPoint3 = new PointGeo(10, 4);

			Assert.Equal(orgin, Topology.ProjectPoint(testPoint1, segment));
			Assert.Equal(orgin, Topology.ProjectPoint(testPoint2, segment));
			Assert.Equal(orgin, Topology.ProjectPoint(testPoint3, segment));
		}

		[Fact()]
		public void TopologyProjectPointReturnsSegmentEndForPointsOutsideSegment() {
			PointGeo orgin = new PointGeo(5, 10);
			PointGeo end = new PointGeo(5, 15);
			Segment<IPointGeo> segment = new Segment<IPointGeo>(orgin, end);

			PointGeo testPoint1 = new PointGeo(5, 17);
			PointGeo testPoint2 = new PointGeo(6, 15);
			PointGeo testPoint3 = new PointGeo(4, 15);

			Assert.Equal(end, Topology.ProjectPoint(testPoint1, segment));
			Assert.Equal(end, Topology.ProjectPoint(testPoint2, segment));
			Assert.Equal(end, Topology.ProjectPoint(testPoint3, segment));
		}


		[Fact()]
		public void TopologyProjectPointReturnsSegmentEndForPointsOutsideSegment2() {
			PointGeo orgin = new PointGeo(10, 5);
			PointGeo end = new PointGeo(15, 5);
			Segment<IPointGeo> segment = new Segment<IPointGeo>(orgin, end);

			PointGeo testPoint1 = new PointGeo(17, 5);
			PointGeo testPoint2 = new PointGeo(15, 6);
			PointGeo testPoint3 = new PointGeo(15, 4);

			Assert.Equal(end, Topology.ProjectPoint(testPoint1, segment));
			Assert.Equal(end, Topology.ProjectPoint(testPoint2, segment));
			Assert.Equal(end, Topology.ProjectPoint(testPoint3, segment));
		}

		[Fact()]
		public void TopologyProjectPointReturnsCorrrectPointOnSegment() {
			PointGeo orgin = new PointGeo(1, 1);
			PointGeo end = new PointGeo(2, 2);
			Segment<IPointGeo> segment = new Segment<IPointGeo>(orgin, end);

			PointGeo testPoint1 = new PointGeo(1, 2);
			PointGeo testPointExpected1 = new PointGeo(1.5,1.5);
			PointGeo testPoint2 = new PointGeo(2, 1);
			PointGeo testPointExpected2 = new PointGeo(1.5,1.5);
			
			Assert.Equal(testPointExpected1, Topology.ProjectPoint(testPoint1, segment));
			Assert.Equal(testPointExpected2, Topology.ProjectPoint(testPoint2, segment));
		}

		[Fact()]
		public void IntersectsReturnsTrueForIntercestiongBBoxes() {
			BBox bbox1 = new BBox() { North = 1, South = -1, East = 1, West = -1 };
			BBox bbox2 = new BBox() { North = 1.5, South = 0.5, East = 1.5, West = 0.5 };

			Assert.True(Topology.Intersects(bbox1, bbox2));
			Assert.True(Topology.Intersects(bbox2, bbox1));
		}

		[Fact()]
		public void IntersectsReturnsFalseForNonIntercestiongBBoxes() {
			BBox bbox1 = new BBox() { North = 1, South = -1, East = 1, West = -1 };
			BBox bbox2 = new BBox() { North = 1.5, South = 1.1, East = 1.5, West = 1.1 };

			Assert.False(Topology.Intersects(bbox1, bbox2));
			Assert.False(Topology.Intersects(bbox2, bbox1));
		}

		[Fact()]
		public void IntersectsReturnsTrueForBBoxInsideLargerOne() {
			BBox large = new BBox() { North = 1, South = -1, East = 1, West = -1 };
			BBox small = new BBox() { North = 0.5, South = -0.5, East = 0.5, West = -0.5 };

			Assert.True(Topology.Intersects(large, small));
			Assert.True(Topology.Intersects(small, large));
		}

		[Fact()]
		public void IntersectsReturnsTrueForIntersectingBBoxesCrossShape() {
			//    ___
			//   |   |
			//  _|___|__
			// |_|___|__|
			//   |___|

			BBox bbox1 = new BBox() { North = 4, South = 0, East = 3, West = 1};
			BBox bbox2 = new BBox() { North = 2, South = 1, East = 4, West = 0};

			Assert.True(Topology.Intersects(bbox1, bbox2));
			Assert.True(Topology.Intersects(bbox2, bbox1));
		}

		[Fact()]
		public void ProjectPointBearingDistanceReturnsCorrectResults1() {
			double angleEps = 0.000001;
			double distanceEps = 0.1;

			// north
			PointGeo testPoint = new PointGeo(12, 34);
			PointGeo target = Topology.ProjectPoint(testPoint, 0, 1000);

			Assert.InRange(target.Longitude, testPoint.Longitude - angleEps, testPoint.Longitude + angleEps);
			Assert.True(target.Latitude > testPoint.Latitude);
			Assert.InRange(Calculations.GetDistance2D(testPoint, target), 1000 - distanceEps, 1000 + distanceEps);
		}

		[Fact()]
		public void ProjectPointBearingDistanceReturnsCorrectResults2() {
			double angleEps = 0.000001;
			double distanceEps = 0.1;

			// east
			PointGeo testPoint = new PointGeo(12, 34);
			PointGeo target = Topology.ProjectPoint(testPoint, 90, 1000);

			Assert.InRange(target.Latitude, testPoint.Latitude - angleEps, testPoint.Latitude + angleEps);
			Assert.True(target.Longitude > testPoint.Longitude);
			Assert.InRange(Calculations.GetDistance2D(testPoint, target), 1000 - distanceEps, 1000 + distanceEps);
		}

		[Fact()]
		public void ProjectPointBearingDistanceReturnsCorrectResults3() {
			double angleEps = 0.000001;
			double distanceEps = 0.1;

			// south
			PointGeo testPoint = new PointGeo(12, 34);
			PointGeo target = Topology.ProjectPoint(testPoint, 180, 1000);

			Assert.InRange(target.Longitude, testPoint.Longitude - angleEps, testPoint.Longitude + angleEps);
			Assert.True(target.Latitude < testPoint.Latitude);
			Assert.InRange(Calculations.GetDistance2D(testPoint, target), 1000 - distanceEps, 1000 + distanceEps);
		}

		[Fact()]
		public void ProjectPointBearingDistanceReturnsCorrectResults4() {
			double angleEps = 0.000001;
			double distanceEps = 0.1;

			// west
			PointGeo testPoint = new PointGeo(12, 34);
			PointGeo target = Topology.ProjectPoint(testPoint, 270, 1000);

			Assert.InRange(target.Latitude, testPoint.Latitude - angleEps, testPoint.Latitude + angleEps);
			Assert.True(target.Longitude < testPoint.Longitude);
			Assert.InRange(Calculations.GetDistance2D(testPoint, target), 1000 - distanceEps, 1000 + distanceEps);
		}

		[Fact()]
		public void GetNodesBetweenPointsReturnsEmptyListForPointsOnPolylineEnds() {
			PointGeo start = new PointGeo(1, 1);
			PointGeo end = new PointGeo(1, 2);
			Polyline<IPointGeo> line = new Polyline<IPointGeo>();
			line.Nodes.Add(start);
			line.Nodes.Add(end);

			var result = Topology.GetNodesBetweenPoints(start, end, line);
			Assert.Equal(0, result.Count());

			result = Topology.GetNodesBetweenPoints(end, start, line);
			Assert.Equal(0, result.Count());
		}

		[Fact()]
		public void GetNodesBetweenPointsReturnsSinglePointForPointBetweenPoints() {
			PointGeo start = new PointGeo(1, 1);
			PointGeo middle = new PointGeo(1.5, 1.5);
			PointGeo end = new PointGeo(1, 2);
			Polyline<IPointGeo> line = new Polyline<IPointGeo>();
			line.Nodes.Add(start);
			line.Nodes.Add(middle);
			line.Nodes.Add(end);

			var result = Topology.GetNodesBetweenPoints(start, end, line);
			Assert.Equal(1, result.Count());
			Assert.Equal(middle, result.Single());

			result = Topology.GetNodesBetweenPoints(end, start, line);
			Assert.Equal(1, result.Count());
			Assert.Equal(middle, result.Single());
		}

		[Fact()]
		public void GetNodesBetweenPointsReturnsSinglePointForPointBetweenPointsDifferentEndPoints() {
			PointGeo start = new PointGeo(1, 1);
			PointGeo middle = new PointGeo(1, 1.5);
			PointGeo end = new PointGeo(1, 2);
			Polyline<IPointGeo> line = new Polyline<IPointGeo>();
			line.Nodes.Add(start);
			line.Nodes.Add(middle);
			line.Nodes.Add(end);

			var result = Topology.GetNodesBetweenPoints(new PointGeo(1, 1.1), new PointGeo(1, 1.9), line);
			Assert.Equal(1, result.Count());
			Assert.Equal(middle, result.Single());

			result = Topology.GetNodesBetweenPoints(new PointGeo(1, 1.9), new PointGeo(1, 1.1), line);
			Assert.Equal(1, result.Count());
			Assert.Equal(middle, result.Single());
		}

		[Fact()]
		public void GetNodesBetweenPointsReturnPointsInCorrectOrder1() {
			PointGeo start = new PointGeo(1, 1);
			PointGeo middle1 = new PointGeo(1, 1.5);
			PointGeo middle2 = new PointGeo(1, 1.6);
			PointGeo middle3 = new PointGeo(1, 1.7);
			PointGeo end = new PointGeo(1, 2);
			Polyline<IPointGeo> line = new Polyline<IPointGeo>();
			line.Nodes.Add(start);
			line.Nodes.Add(middle1);
			line.Nodes.Add(middle2);
			line.Nodes.Add(middle3);
			line.Nodes.Add(end);

			var result = Topology.GetNodesBetweenPoints(new PointGeo(1, 1.1), new PointGeo(1, 1.9), line).ToList();
			Assert.Equal(3, result.Count());
			Assert.Equal(middle1, result[0]);
			Assert.Equal(middle2, result[1]);
			Assert.Equal(middle3, result[2]);


			result = Topology.GetNodesBetweenPoints(new PointGeo(1, 1.9), new PointGeo(1, 1.1), line).ToList();
			Assert.Equal(3, result.Count());
			Assert.Equal(middle3, result[0]);
			Assert.Equal(middle2, result[1]);
			Assert.Equal(middle1, result[2]);
		}

		[Fact()]
		public void GetNodesBetweenPointsReturnPointsInCorrectOrder2() {
			PointGeo start = new PointGeo(1, 1);
			PointGeo middle1 = new PointGeo(1, 1.5);
			PointGeo middle2 = new PointGeo(1, 1.6);
			PointGeo middle3 = new PointGeo(1, 1.7);
			PointGeo end = new PointGeo(1, 2);
			Polyline<IPointGeo> line = new Polyline<IPointGeo>();
			line.Nodes.Add(start);
			line.Nodes.Add(middle1);
			line.Nodes.Add(middle2);
			line.Nodes.Add(middle3);
			line.Nodes.Add(end);

			var result = Topology.GetNodesBetweenPoints(new PointGeo(1, 1.55), new PointGeo(1, 1.9), line).ToList();
			Assert.Equal(2, result.Count());
			Assert.Equal(middle2, result[0]);
			Assert.Equal(middle3, result[1]);


			result = Topology.GetNodesBetweenPoints(new PointGeo(1, 1.9), new PointGeo(1, 1.55), line).ToList();
			Assert.Equal(2, result.Count());
			Assert.Equal(middle3, result[0]);
			Assert.Equal(middle2, result[1]);
		}
	}
}
