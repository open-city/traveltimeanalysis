using LK.GeoUtils;
using Xunit;
using System;
using LK.GeoUtils.Geometry;
using OSMUtils.OSMDatabase;
using System.Collections.Generic;

namespace OSMUtils.Tests
{
    
    
    /// <summary>
    ///This is a test class for ExtensionsTest and is intended
    ///to contain all ExtensionsTest Unit Tests
    ///</summary>
	
	public class ExtensionsTest {
		[Fact()]
		public void ExtensionsPolygonAddWaysAcceptsCollectionOfOSMWays() {
			OSMDB db = new OSMDB();
		
			OSMNode p1 = new OSMNode(1, 10, 15);
			OSMNode p2 = new OSMNode(2, 0, 15);
			OSMNode p3 = new OSMNode(3, 0, 0);

			OSMWay way1 = new OSMWay(10);
			OSMWay way2 = new OSMWay(11);

			db.Nodes.Add(p1);
			db.Nodes.Add(p2);
			db.Nodes.Add(p3);

			way1.Nodes.Add(p1.ID);
			way1.Nodes.Add(p2.ID);
			way1.Nodes.Add(p3.ID);

			way2.Nodes.Add(p3.ID);
			way2.Nodes.Add(p1.ID);
		
			Polygon<OSMNode> target = new Polygon<OSMNode>();
			target.AddWays(new OSMWay[] {way1, way2}, db);

			Assert.Equal(3, target.VerticesCount);
			CompareVerticesLists(new IPointGeo[] { p3, p2, p1 }, target.Vertices);
		}

		[Fact()]
		public void PolygonConstructorAcceptsCollectionOfOSMWaysEvenIfTheyAreRevesed() {
			OSMDB db = new OSMDB();

			OSMNode p1 = new OSMNode(1, 10, 15);
			OSMNode p2 = new OSMNode(2, 0, 15);
			OSMNode p3 = new OSMNode(3, 0, 0);
			OSMNode p4 = new OSMNode(4, 10, 0);


			OSMWay way1 = new OSMWay(10);
			OSMWay way2 = new OSMWay(11);

			db.Nodes.Add(p1);
			db.Nodes.Add(p2);
			db.Nodes.Add(p3);
			db.Nodes.Add(p4);

			way1.Nodes.Add(p1.ID);
			way1.Nodes.Add(p2.ID);
			way1.Nodes.Add(p3.ID);

			way2.Nodes.Add(p1.ID);
			way2.Nodes.Add(p4.ID);
			way2.Nodes.Add(p3.ID);


			Polygon<OSMNode> target = new Polygon<OSMNode>();
			target.AddWays(new OSMWay[] { way1, way2 }, db);

			Assert.Equal(4, target.VerticesCount);
			CompareVerticesLists(new IPointGeo[] { p1, p2, p3, p4 }, target.Vertices);
		}

		[Fact()]
		public void PolygonConstructorAcceptsCollectionOfOSMWaysFirstWayReversed() {
			OSMDB db = new OSMDB();

			OSMNode p1 = new OSMNode(1, 10, 15);
			OSMNode p2 = new OSMNode(2, 0, 15);
			OSMNode p3 = new OSMNode(3, 0, 0);
			OSMNode p4 = new OSMNode(4, 10, 0);

			OSMWay way1 = new OSMWay(10);
			OSMWay way2 = new OSMWay(11);
			OSMWay way3 = new OSMWay(12);

			db.Nodes.Add(p1);
			db.Nodes.Add(p2);
			db.Nodes.Add(p3);
			db.Nodes.Add(p4);

			way1.Nodes.Add(p1.ID);
			way1.Nodes.Add(p2.ID);
			way1.Nodes.Add(p3.ID);

			way2.Nodes.Add(p1.ID);
			way2.Nodes.Add(p4.ID);

			way3.Nodes.Add(p3.ID);
			way3.Nodes.Add(p4.ID);

			Polygon<OSMNode> target = new Polygon<OSMNode>();
			target.AddWays(new OSMWay[] { way1, way2, way3 }, db);

			Assert.Equal(4, target.VerticesCount);
			CompareVerticesLists(new IPointGeo[] { p3, p2, p1, p4 }, target.Vertices);
		}


		[Fact()]
		public void PolygonConstructorThrowsExceptionIfWaysDontFormClosedPolygon() {
			OSMDB db = new OSMDB();

			OSMNode p1 = new OSMNode(1, 10, 15);
			OSMNode p2 = new OSMNode(2, 0, 15);
			OSMNode p3 = new OSMNode(3, 0, 0);
			OSMNode p4 = new OSMNode(4, 10, 0);

			OSMWay way1 = new OSMWay(10);
			OSMWay way2 = new OSMWay(11);

			db.Nodes.Add(p1);
			db.Nodes.Add(p2);
			db.Nodes.Add(p3);
			db.Nodes.Add(p4);

			way1.Nodes.Add(p1.ID);
			way1.Nodes.Add(p2.ID);
			way1.Nodes.Add(p3.ID);

			way2.Nodes.Add(p3.ID);
			way2.Nodes.Add(p4.ID);


			Polygon<OSMNode> target = new Polygon<OSMNode>();
            Assert.Throws<ArgumentException>(delegate { target.AddWays(new OSMWay[] { way1, way2 }, db); });
		}

		[Fact()]
		public void PolygonConstructorThrowsExceptionIfInitializedWithSingleOpenWay() {
			OSMDB db = new OSMDB();

			OSMNode p1 = new OSMNode(1, 10, 15);
			OSMNode p2 = new OSMNode(2, 0, 15);
			OSMNode p3 = new OSMNode(3, 0, 0);

			OSMWay way1 = new OSMWay(10);

			db.Nodes.Add(p1);
			db.Nodes.Add(p2);
			db.Nodes.Add(p3);

			way1.Nodes.Add(p1.ID);
			way1.Nodes.Add(p2.ID);
			way1.Nodes.Add(p3.ID);

			Polygon<OSMNode> target = new Polygon<OSMNode>();
            Assert.Throws<ArgumentException>(delegate { target.AddWays(new OSMWay[] { way1 }, db); });
		}

		[Fact()]
		public void PolygonConstructorThrowsExceptionIfWaysArentConnected() {
			OSMDB db = new OSMDB();

			OSMNode p1 = new OSMNode(1, 10, 15);
			OSMNode p2 = new OSMNode(2, 0, 15);
			OSMNode p3 = new OSMNode(3, 0, 0);
			OSMNode p4 = new OSMNode(4, 10, 1);

			OSMWay way1 = new OSMWay(10);
			OSMWay way2 = new OSMWay(11);

			db.Nodes.Add(p1);
			db.Nodes.Add(p2);
			db.Nodes.Add(p3);
			db.Nodes.Add(p4);

			way1.Nodes.Add(p1.ID);
			way1.Nodes.Add(p2.ID);

			way2.Nodes.Add(p3.ID);
			way2.Nodes.Add(p4.ID);

			Polygon<OSMNode> target = new Polygon<OSMNode>();
            Assert.Throws<ArgumentException>(delegate { target.AddWays(new OSMWay[] { way1, way2 }, db); });
		}

		void CompareVerticesLists(IList<IPointGeo> expected, IList<IPointGeo> actual) {
			Assert.Equal(expected.Count, actual.Count);

			for (int i = 0; i < expected.Count; i++) {
				Assert.Equal(expected[i], actual[i]);
			}
		}
	}
}
