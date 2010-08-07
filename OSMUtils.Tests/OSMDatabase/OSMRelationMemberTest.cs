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

namespace OSMUtils.Tests
{
    
    
    /// <summary>
    ///This is a test class for OSMRelationMemberTest and is intended
    ///to contain all OSMRelationMemberTest Unit Tests
    ///</summary>
	
	public class OSMRelationMemberTest {
		/// <summary>
		///A test for OSMRelationMember Constructor
		///</summary>
		[Fact()]
		public void OSMRelationMemberConstructorSetsAllProperties() {
			OSMRelationMemberType type = OSMRelationMemberType.way;
			int reference = 1273;
			string role = "boundary";

			OSMRelationMember target = new OSMRelationMember(type, reference, role);

			Assert.Equal(type, target.Type);
			Assert.Equal(reference, target.Reference);
			Assert.Equal(role, target.Role);
		}

		[Fact()]
		public void OSMRelationMemberRolePropertyCanBeSetAndGet() {
			OSMRelationMemberType type = OSMRelationMemberType.way;
			int reference = 1273;
			OSMRelationMember target = new OSMRelationMember(type, reference, null);
			
			string role = "boundary";
			target.Role = role;

			Assert.Equal(role, target.Role);
		}
	}
}
