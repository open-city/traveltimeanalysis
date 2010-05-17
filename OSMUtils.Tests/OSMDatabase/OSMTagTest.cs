using LK.OSMUtils.OSMDatabase;
using Xunit;
using System;

namespace OSMUtils.Tests
{
    
    
    /// <summary>
    ///This is a test class for OSMTagTest and is intended
    ///to contain all OSMTagTest Unit Tests
    ///</summary>
	
	public class OSMTagTest {
		[Fact()]
		public void OSMTagConstructorSetsKeyAndValue() {
			string key = "test-key";
			string value = "test-value";

			OSMTag target = new OSMTag(key, value);

			Assert.Equal(key, target.Key);
			Assert.Equal(value, target.Value);
		}

		[Fact()]
		public void OSMTagConstructorThrowsExceptionIfKeyIsNull() {
			string key = null;
			string value = "test-value";

            Assert.Throws<ArgumentException>(delegate { new OSMTag(key, value); });
		}

		[Fact()]
		public void OSMTagConstructorThrowsExceptionIfKeyIsEmpty() {
			string key = "";
			string value = "test-value";

            Assert.Throws<ArgumentException>(delegate { new OSMTag(key, value); });
		}

		[Fact()]
		public void OSMTagConstructorThrowsExceptionIfValueIsNull() {
			string key = "test-key";
			string value = null;

            Assert.Throws<ArgumentNullException>(delegate { new OSMTag(key, value); });
		}

		[Fact()]
		public void OSMTagToStringReturnsCorrectStringRepresentationOfTheObject() {
			string key = "test-key";
			string value = "test-value";
			string stringRepresentation = "test-key = test-value";

			OSMTag target = new OSMTag(key, value);

			Assert.Equal(stringRepresentation, target.ToString());
		}

		[Fact()]
		public void OSMTagEqualsComparesKeyAndValue() {
			OSMTag target = new OSMTag("test-key", "test-value");
			OSMTag equalsTarget = new OSMTag("test-key", "test-value");

			Assert.True(target.Equals(equalsTarget));

			OSMTag differentKeyTarget = new OSMTag("different-key", "test-value");
			Assert.False(target.Equals(differentKeyTarget));

			OSMTag differentValueTarget = new OSMTag("test-key", "different-value");
			Assert.False(target.Equals(differentValueTarget));
		}

		[Fact()]
		public void OSMTagGetHashCodeComputeHashFromKeyAndValue() {
			string key = "test-key";
			string value = "test-value";
			int expectedHashCode = unchecked( key.GetHashCode()*83 + value.GetHashCode() );

			OSMTag target = new OSMTag(key, value);

			Assert.Equal(expectedHashCode, target.GetHashCode());
		}
	}
}
