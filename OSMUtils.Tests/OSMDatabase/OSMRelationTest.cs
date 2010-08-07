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

using LK.OSMUtils.OSMDatabase;
using Xunit;
using System;
using System.Collections.Generic;

namespace OSMUtils.Tests
{
    /// <summary>
    ///This is a test class for OSMRelationTest and is intended
    ///to contain all OSMRelationTest Unit Tests
    ///</summary>
	
	public class OSMRelationTest {
		/// <summary>
		///A test for OSMRelation Constructor
		///</summary>
		[Fact()]
		public void OSMRelationConstructorInitializesAllProperties() {
			int id = 1273;

			OSMRelation target = new OSMRelation(id);

			Assert.Equal(id, target.ID);
			Assert.NotNull(target.Members);
		}

	}
}
