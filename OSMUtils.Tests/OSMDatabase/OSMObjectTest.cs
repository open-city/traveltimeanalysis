using LK.OSMUtils.OSMDatabase;
using Xunit;
using System;

namespace OSMUtils.Tests
{
    
    
    /// <summary>
    ///This is a test class for OSMObjectTest and is intended
    ///to contain all OSMObjectTest Unit Tests
    ///</summary>
	
	public class OSMObjectTest {
		private class OSMObjectWrapper : OSMObject {
			public OSMObjectWrapper(int id)
				: base(id) {
			}
		}


		[Fact()]
		public void OSMObjectConstructorInitializesProperties() {
			int id = 1374;

			OSMObject target = new OSMObjectWrapper(id);

			Assert.Equal(id, target.ID);
			Assert.NotNull(target.Tags);
		}

		[Fact()]
		public void OSMObjectToStringReturnsTheStringRepresentationOfTheID() {
			int id = 1374;

			OSMObjectWrapper target = new OSMObjectWrapper(id);

			Assert.Equal(id.ToString(), target.ToString());
		}

		[Fact()]
		public void OSMObjectGetHashCodeComputeHashFromTheID() {
			int id = 1374;

			OSMObjectWrapper target = new OSMObjectWrapper(id);

			Assert.Equal(id.GetHashCode(), target.GetHashCode());
		}

		[Fact()]
		public void OSMObjectEqualsComparesObjcetsByID() {
			OSMObjectWrapper target = new OSMObjectWrapper(1374);
			OSMObjectWrapper equalsTaregt = new OSMObjectWrapper(1374);

			Assert.True(target.Equals(equalsTaregt));


			OSMObjectWrapper otherObject = new OSMObjectWrapper(1);

			Assert.False(target.Equals(otherObject));
		}
	}
}
