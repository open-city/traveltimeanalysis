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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GeoUtils;
using LK.GeoUtils.Geometry;

namespace GeoUtils.Tests {
	public class SegmentTest {
		[Fact()]
		public void SegmentConstructorSetsStartAndEndPoint() {
			PointGeo startPoint = new PointGeo(21, 34);
			PointGeo endPoint = new PointGeo(26, 31);

			Segment<PointGeo> target = new Segment<PointGeo>(startPoint, endPoint);

			Assert.Equal(startPoint, target.StartPoint);
			Assert.Equal(endPoint, target.EndPoint);
		}

		[Fact()]
		public void SegmentLengthReturnsDistanceBetweenPoints() {
			PointGeo startPoint = new PointGeo(21, 34);
			PointGeo endPoint = new PointGeo(26, 31);

			Segment<PointGeo> target = new Segment<PointGeo>(startPoint, endPoint);

			double expectedLength = Calculations.GetDistance2D(startPoint, endPoint);
			Assert.InRange(target.Length, expectedLength - Calculations.EpsLength, expectedLength + Calculations.EpsLength);
		}

		[Fact()]
		public void SegmentEqualsReturnsTrueForSegmentsWithTheSameEndPoints() {
			PointGeo startPoint = new PointGeo(21, 34);
			PointGeo endPoint = new PointGeo(26, 31);

			Segment<PointGeo> target = new Segment<PointGeo>(startPoint, endPoint);
			Segment<PointGeo> theSame = new Segment<PointGeo>(startPoint, endPoint);

			Assert.True(target.Equals(theSame));
		}

		[Fact()]
		public void SegmentEqualsReturnsFalseForNull() {
			PointGeo startPoint = new PointGeo(21, 34);
			PointGeo endPoint = new PointGeo(26, 31);

			Segment<PointGeo> target = new Segment<PointGeo>(startPoint, endPoint);

			Assert.False(target.Equals(null));
		}

		[Fact()]
		public void SegmentEqualsReturnsFalseForSegmentsWithTheDifferentEndPoints() {
			PointGeo startPoint = new PointGeo(21, 34);
			PointGeo startPoint2 = new PointGeo(21.1, 34.1);

			PointGeo endPoint = new PointGeo(26, 31);

			Segment<PointGeo> target = new Segment<PointGeo>(startPoint, endPoint);
			Segment<PointGeo> other = new Segment<PointGeo>(startPoint2, endPoint);

			Assert.False(target.Equals(other));
		}
	}
}
