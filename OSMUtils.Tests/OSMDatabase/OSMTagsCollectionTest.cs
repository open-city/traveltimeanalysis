using LK.OSMUtils.OSMDatabase;
using Xunit;
using System;

namespace OSMUtils.Tests
{
    
    
    /// <summary>
    ///This is a test class for OSMTagsCollectionTest and is intended
    ///to contain all OSMTagsCollectionTest Unit Tests
    ///</summary>
	
	public class OSMTagsCollectionTest {
		[Fact()]
		public void OSMTagsCollectionConstructorAcceptsNoParameters() {
			OSMTagsCollection target = new OSMTagsCollection();

			Assert.Equal(0, target.Count);
		}

		[Fact()]
		public void OSMTagsCollectionAddAddsOSMTag() {
			OSMTagsCollection target = new OSMTagsCollection();
			OSMTag toAdd = new OSMTag("test-key", "test-value");

			Assert.Equal(0, target.Count);

			target.Add(toAdd);
			Assert.Equal(1, target.Count);
		}

		[Fact()]
		public void OSMTagsCollectionAddThrowsExceprionIfTagWithTheSameKeyIsAlreadyPresent() {
			OSMTagsCollection target = new OSMTagsCollection();
			string key = "test-key";
			OSMTag toAdd = new OSMTag(key, "test-value");
			target.Add(toAdd);

			OSMTag anotherTag = new OSMTag(key, "some-value");

            Assert.Throws<ArgumentException>(delegate { target.Add(anotherTag); });
		}

		[Fact()]
		public void OSMTagsCollectionStringIndexerReturnsCorrectOSMTagByKey() {
			OSMTagsCollection target = new OSMTagsCollection();
			string key = "test-key";
			OSMTag toAdd = new OSMTag(key, "test-value");
			target.Add(toAdd);

			Assert.Same(toAdd, target[key]);
		}

		[Fact()]
		public void OSMTagsCollectionStringIndexerThrowsExceptionIfKeyIsNotPresent() {
			OSMTagsCollection target = new OSMTagsCollection();
			OSMTag toAdd = new OSMTag("test-key", "test-value");
			target.Add(toAdd);

            Assert.Throws<ArgumentException>(delegate { OSMTag tag = target["another-key"]; });
		}

		[Fact()]
		public void OSMTagsCollectionHasTagReturnsCorrectValues() {
			OSMTagsCollection target = new OSMTagsCollection();
			string key = "test-key";
			OSMTag toAdd = new OSMTag(key, "test-value");
			target.Add(toAdd);

			Assert.True(target.ContainsTag(key));
			Assert.False(target.ContainsTag("another-key"));
		}

		[Fact()]
		public void OSMTagsCollectionImplementsIEnumerable() {
			OSMTagsCollection target = new OSMTagsCollection();
			OSMTag first = new OSMTag("test-key", "test-value");
			OSMTag second = new OSMTag("test-key-2", "another-test-value");
			target.Add(first);
			target.Add(second);

			int counter = 0;
			foreach (var tag in target) {
				counter++;
			}

			Assert.Equal(2, counter);
		}

		[Fact()]
		public void OSMTagsCollectionGetEnumerableDoesNotThrowExceptionOnEmptyCollection() {
			OSMTagsCollection target = new OSMTagsCollection();

			int counter = 0;
			foreach (var tag in target) {
				counter++;
			}

			Assert.Equal(0, counter);
		}

		[Fact()]
		public void OSMTagsCollectionRemoveCanRemoveTagFromCollection() {
			OSMTagsCollection target = new OSMTagsCollection();
			OSMTag tag = new OSMTag("test-key", "test-value");

			target.Add(tag);

			Assert.True(target.Remove(tag));
			Assert.Equal(0, target.Count);
		}

		[Fact()]
		public void OSMTagsCollectionRemoveReturnsFalseWhenRemovingItemFromEmptyCollection() {
			OSMTagsCollection target = new OSMTagsCollection();
			OSMTag tag = new OSMTag("test-key", "test-value");

			Assert.False(target.Remove(tag));
		}

		[Fact()]
		public void OSMTagsCollectionRemoveAllDoesntThrowAnExceptionOnEmptyCollection() {
			OSMTagsCollection target = new OSMTagsCollection();

			target.RemoveAll();

			Assert.Equal(0, target.Count);
		}

		[Fact()]
		public void OSMTagsCollectionRemoveAllRemovesAllTagsFromTheCollection() {
			OSMTagsCollection target = new OSMTagsCollection();
			OSMTag first = new OSMTag("test-key", "test-value");
			OSMTag second = new OSMTag("test-key-2", "another-test-value");
			target.Add(first);
			target.Add(second);

			target.RemoveAll();

			Assert.Equal(0, target.Count);
		}
	}
}
