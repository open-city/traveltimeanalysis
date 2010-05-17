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
