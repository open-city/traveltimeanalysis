using OSMUtils.OSMDatabase;
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
