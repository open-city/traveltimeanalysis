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

using LK.GPXUtils;

namespace GPXUtils.Tests {
	public class GPXTrackSegmentTest {
		[Fact()]
		public void GPXTrackSegmentConstructorCreatesEmptySegment() {
			GPXTrackSegment target = new GPXTrackSegment();

			Assert.Equal(0, target.NodesCount);
		}

		[Fact()]
		public void GPXTrackSegmentConstructorAcceptsListOfGPXPoints() {
			GPXPoint pt1 = new GPXPoint(10.4, 5.2);
			GPXPoint pt2 = new GPXPoint(10.8, 5.3);

			GPXTrackSegment target = new GPXTrackSegment(new GPXPoint[] { pt1, pt2 });

			Assert.Equal(2, target.NodesCount);
			Assert.Equal(pt1, target.Nodes[0]);
			Assert.Equal(pt2, target.Nodes[1]);
		}
	}
}
