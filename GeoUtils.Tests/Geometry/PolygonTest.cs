using LK.GeoUtils.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace GeoUtils.Tests
{
  public class PolygonTest {
		[Fact()]
		public void PolygonConstructorInitializesEmptyPolygon() {
			Polygon<PointGeo> target = new Polygon<PointGeo>();

			Assert.Equal(0, target.VerticesCount);
		}

		[Fact()]
		public void PolygonConstructorAcceptsCollectionOfVertices() {
			PointGeo p1 = new PointGeo(1.0, 1.0);
			PointGeo p2 = new PointGeo(2.0, 1.0);
			PointGeo p3 = new PointGeo(2.0, 2.0);
			PointGeo p4 = new PointGeo(1.0, 2.0);
			List<IPointGeo> nodes = new List<IPointGeo>(new IPointGeo[] { p1, p2, p3, p4 });

			Polygon<PointGeo> target = new Polygon<PointGeo>(nodes);

			CompareVerticesLists(nodes, target.Vertices);
		}


		[Fact()]
		public void PolygonConstructorThrowsExceptionWhenVerticesContainsDuplicateNodes() {
			PointGeo p1 = new PointGeo(1.0, 1.0);
			PointGeo p2 = new PointGeo(2.0, 1.0);
			PointGeo p3 = new PointGeo(2.0, 2.0);
			PointGeo p4 = new PointGeo(1.0, 2.0);

			List<IPointGeo> nodes = new List<IPointGeo>(new IPointGeo[] { p1, p2, p3, p2, p4 });

			Assert.Throws<ArgumentException>(delegate { new Polygon<PointGeo>(nodes); });
		}

		[Fact()]
		public void PolygonVerticesReturnsDataInCorrectOrder() {
			PointGeo p1 = new PointGeo(1.0, 1.0);
			PointGeo p2 = new PointGeo(2.0, 1.0);
			PointGeo p3 = new PointGeo(2.0, 2.0);
			PointGeo p4 = new PointGeo(1.0, 2.0);
			List<IPointGeo> nodes = new List<IPointGeo>(new IPointGeo[] { p1, p2, p3, p4 });

			Polygon<PointGeo> target = new Polygon<PointGeo>(nodes);

			CompareVerticesLists(nodes, target.Vertices);
		}

		[Fact()]
		public void PolygonBoundingBoxReturnsCorrectData() {
			PointGeo p1 = new PointGeo(1.0, 1.0);
			PointGeo p2 = new PointGeo(2.0, 1.0);
			PointGeo p3 = new PointGeo(2.0, 2.0);
			PointGeo p4 = new PointGeo(1.0, 2.0);
			List<IPointGeo> nodes = new List<IPointGeo>(new IPointGeo[] { p1, p2, p3, p4 });

			Polygon<PointGeo> target = new Polygon<PointGeo>(nodes);

			Assert.Equal(2, target.BoundingBox.North);
			Assert.Equal(1, target.BoundingBox.South);
			Assert.Equal(2, target.BoundingBox.East);
			Assert.Equal(1, target.BoundingBox.West);
		}
		
		[Fact()]
		public void PolygonAddVertexAddsVertexToTheEnd() {
			PointGeo p1 = new PointGeo(1.0, 1.0);
			PointGeo p2 = new PointGeo(2.0, 1.0);
			PointGeo p3 = new PointGeo(2.0, 2.0);
			PointGeo p4 = new PointGeo(1.0, 2.0);
			List<IPointGeo> nodes = new List<IPointGeo>(new IPointGeo[] { p1, p2, p3});

			Polygon<PointGeo> target = new Polygon<PointGeo>(nodes);
			target.AddVertex(p4);

			nodes.Add(p4);
			CompareVerticesLists(nodes, target.Vertices);
		}


		[Fact()]
		public void AddVertexThrowsExceptionWhenAddingDuplicateNode() {
			PointGeo p1 = new PointGeo(1.0, 1.0);
			PointGeo p2 = new PointGeo(2.0, 1.0);
			PointGeo p3 = new PointGeo(2.0, 2.0);
			PointGeo p4 = new PointGeo(1.0, 2.0);
			List<IPointGeo> nodes = new List<IPointGeo>(new IPointGeo[] { p1, p2, p3, p4 });

			Polygon<PointGeo> target = new Polygon<PointGeo>(nodes);

            Assert.Throws<ArgumentException>(delegate { target.AddVertex(p2); });
		}

		[Fact()]
		public void RemoveVertexReturnsTrueIfPolygonContainsGivenVertex() {
			PointGeo p1 = new PointGeo(1.0, 1.0);
			PointGeo p2 = new PointGeo(2.0, 1.0);
			PointGeo p3 = new PointGeo(2.0, 2.0);
			PointGeo p4 = new PointGeo(1.0, 2.0);
			List<IPointGeo> nodes = new List<IPointGeo>(new IPointGeo[] { p1, p2, p3, p4 });

			Polygon<PointGeo> target = new Polygon<PointGeo>(nodes);

			Assert.True(target.RemoveVertex(p2));
			Assert.False(target.Vertices.Contains(p2));
			Assert.Equal(nodes.Count - 1, target.Vertices.Count);
		}
		
		[Fact()]
		public void PolygonVerticesReturnsCorrectData() {
			PointGeo p1 = new PointGeo(1.0, 1.0);
			PointGeo p2 = new PointGeo(2.0, 1.0);
			PointGeo p3 = new PointGeo(2.0, 2.0);
			PointGeo p4 = new PointGeo(1.0, 2.0);
			List<IPointGeo> nodes = new List<IPointGeo>(new IPointGeo[] { p1, p2, p3, p4 });

			Polygon<PointGeo> target = new Polygon<PointGeo>(nodes);

			Assert.Equal(nodes.Count, target.Vertices.Count);
		}

		[Fact()]
		public void PolygonIsInsideReturnsTrueForPointInside() {
			PointGeo p1 = new PointGeo(15, 10, 0);
			PointGeo p2 = new PointGeo(15, -10, 0);
			PointGeo p3 = new PointGeo(-15, -10, 0);
			PointGeo p4 = new PointGeo(-15, 10, 0);

			PointGeo pTest = new PointGeo(0, 0, 0);

			Polygon<PointGeo> target = new Polygon<PointGeo>(new IPointGeo[] { p1, p2, p3, p4 });

			Assert.True(target.IsInside(pTest));
		}
		
		//TODO

		//[Fact()]
		//public void PolygonIsInsideReturnsTrueForPointOnBoundary() {
		//  PointGeo p1 = new PointGeo(15, 10, 0);
		//  PointGeo p2 = new PointGeo(15, -10, 0);
		//  PointGeo p3 = new PointGeo(-15, -10, 0);
		//  PointGeo p4 = new PointGeo(-15, 10, 0);

		//  PointGeo pTest1 = new PointGeo(15, 0, 0);
		//  PointGeo pTest2 = new PointGeo(-15, 0, 0);
		//  PointGeo pTest3 = new PointGeo(0, 10, 0);
		//  PointGeo pTest4 = new PointGeo(0, -10, 0);


		//  Polygon<PointGeo> target = new Polygon<PointGeo>(new IPointGeo[] { p1, p2, p3, p4 });

		//  Assert.IsTrue(target.IsInside(pTest1));
		//  Assert.IsTrue(target.IsInside(pTest2));
		//  Assert.IsTrue(target.IsInside(pTest3));
		//  Assert.IsTrue(target.IsInside(pTest4));
		//}

		[Fact()]
		public void PolygonIsInsideReturnsTrueForPointOutside() {
			PointGeo p1 = new PointGeo(15, 10, 0);
			PointGeo p2 = new PointGeo(15, -10, 0);
			PointGeo p3 = new PointGeo(-15, -10, 0);
			PointGeo p4 = new PointGeo(-15, 10, 0);

			PointGeo pTest = new PointGeo(-20, 15, 0);

			Polygon<PointGeo> target = new Polygon<PointGeo>(new IPointGeo[] { p1, p2, p3, p4 });

			Assert.False(target.IsInside(pTest));
		}

		void CompareVerticesLists(IList<IPointGeo> expected, IList<IPointGeo> actual) {
			Assert.Equal(expected.Count, actual.Count);

			for (int i = 0; i < expected.Count; i++) {
				Assert.Equal(expected[i], actual[i]);
			}
		}
	}
}
