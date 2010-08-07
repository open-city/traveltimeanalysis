//  Travel Time Analysis project
//  Copyright (C) 2010 Lukas Kabrt
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using LK.GeoUtils.Geometry;
using System;
using System.Collections.Generic;

using Xunit;

using LK.GeoUtils;

namespace GeoUtils.Tests {
	public class PolylineTest {
		[Fact()]
		public void PolylineConstructorInitializesInteralFields() {
			Polyline<PointGeo> target = new Polyline<PointGeo>();

			Assert.NotNull(target.Nodes);
		}

		[Fact()]
		public void PolylineNodesGetsListOfNodes() {
			PointGeo p1 = new PointGeo(1, 2);
			PointGeo p2 = new PointGeo(3, 4);

			Polyline<PointGeo> target = new Polyline<PointGeo>();
			target.Nodes.Add(p1);
			target.Nodes.Add(p2);

			Assert.Equal(p1, target.Nodes[0]);
			Assert.Equal(p2, target.Nodes[1]);
		}

		[Fact()]
		public void PolylineNodesCountReturnsCorrectValue() {
			PointGeo p1 = new PointGeo(1, 2);
			PointGeo p2 = new PointGeo(3, 4);

			Polyline<PointGeo> target = new Polyline<PointGeo>();
			target.Nodes.Add(p1);
			target.Nodes.Add(p2);

			Assert.Equal(2, target.NodesCount);
		}

		[Fact()]
		public void PolylineLengthReturnsLengthOfThePolyline() {
			PointGeo p1 = new PointGeo(1, 2);
			PointGeo p2 = new PointGeo(3, 4);

			IPolyline<IPointGeo> target = new Polyline<IPointGeo>();
			target.Nodes.Add(p1);
			target.Nodes.Add(p2);

			double expectedLength = Calculations.GetLength(target);

			Assert.InRange(target.Length, expectedLength - Calculations.EpsLength, expectedLength + Calculations.EpsLength);
		}

		[Fact()]
		public void PolylineLengthReturnsLengthOfThePolylineAfterPolylineChange() {
			PointGeo p1 = new PointGeo(1, 2);
			PointGeo p2 = new PointGeo(3, 4);
			PointGeo p3 = new PointGeo(5, 6);

			IPolyline<IPointGeo> target = new Polyline<IPointGeo>();
			target.Nodes.Add(p1);
			target.Nodes.Add(p2);
			double length = target.Length;

			//change polyline
			target.Nodes.Add(p3);
			double expectedLength = Calculations.GetLength(target);

			Assert.InRange(target.Length, expectedLength - Calculations.EpsLength, expectedLength + Calculations.EpsLength);
		}

		[Fact()]
		public void PolylineSegmentsReturnsListOfSegments() {
			PointGeo pt1 = new PointGeo(1, 2);
			PointGeo pt2 = new PointGeo(3, 4);
			PointGeo pt3 = new PointGeo(5, 6);

			Polyline<IPointGeo> target = new Polyline<IPointGeo>();
			target.Nodes.Add(pt1);
			target.Nodes.Add(pt2);
			target.Nodes.Add(pt3);

			IList<Segment<IPointGeo>> segments = target.Segments;

			Assert.Equal(2, segments.Count);

			Assert.Equal(pt1, segments[0].StartPoint);
			Assert.Equal(pt2, segments[0].EndPoint);

			Assert.Equal(pt2, segments[1].StartPoint);
			Assert.Equal(pt3, segments[1].EndPoint);
		}

		[Fact()]
		public void PolylineSegmentsReturnsListOfSegmentsAfterPolylineChange() {
			PointGeo pt1 = new PointGeo(1, 2);
			PointGeo pt2 = new PointGeo(3, 4);
			PointGeo pt3 = new PointGeo(5, 6);

			Polyline<IPointGeo> target = new Polyline<IPointGeo>();
			target.Nodes.Add(pt1);
			target.Nodes.Add(pt2);

			IList<Segment<IPointGeo>> segments = target.Segments;

			target.Nodes.Add(pt3);
			segments = target.Segments;

			Assert.Equal(2, segments.Count);
		}
	}
}
