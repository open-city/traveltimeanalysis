using LK.OSMUtils.OSMDatabase;
using Xunit;
using System;

namespace OSMUtils.Tests
{
    
    
    /// <summary>
    ///This is a test class for OSMObjectCollectionTest and is intended
    ///to contain all OSMObjectCollectionTest Unit Tests
    ///</summary>
	
	public class OSMObjectCollectionTest {
		[Fact()]
		public void OSMOjectCollectionConstructorInitializesIntrenalStorage() {
			OSMObjectCollection<OSMNode> target = new OSMObjectCollection<OSMNode>();

			//If the internal storage isn't initialized, an exception is thrown
			Assert.Equal(0, target.Count);
		}

		[Fact()]
		public void OSMObjectCollectionAddAddObjectIntoCollection() {
			OSMObjectCollection<OSMNode> target = new OSMObjectCollection<OSMNode>();
			OSMNode node = new OSMNode(1254, 1.0, 2.0);

			target.Add(node);

			//If the internal storage isn't initialized, an exception is thrown
			Assert.Equal(1, target.Count);
		}

		[Fact()]
		public void OSMObjectCollectionAddingDuplicateItemsThrowsException() {
			OSMObjectCollection<OSMNode> target = new OSMObjectCollection<OSMNode>();
			OSMNode node = new OSMNode(1254, 1.0, 2.0);
			OSMNode nodeDuplicate = new OSMNode(1254, 3.0, 4.0);

			target.Add(node);

            Assert.Throws<ArgumentException>(delegate { target.Add(nodeDuplicate); });
		}

		[Fact()]
		public void OSMObjectCollectionAddingNullThrowsException() {
			OSMObjectCollection<OSMNode> target = new OSMObjectCollection<OSMNode>();
			OSMNode node = null;

            Assert.Throws<ArgumentNullException>(delegate { target.Add(node); });
		}

		[Fact()]
		public void OSMObjectCollectionRemoveCanRemoveObjectFromCollection() {
			OSMObjectCollection<OSMNode> target = new OSMObjectCollection<OSMNode>();
			OSMNode node = new OSMNode(1254, 1.0, 2.0);

			target.Add(node);
			
			Assert.True(target.Remove(node));
			Assert.Equal(0, target.Count);
		}

		[Fact()]
		public void OSMObjectCollectionRemoveAllRemovesAllObjects() {
			OSMObjectCollection<OSMNode> target = new OSMObjectCollection<OSMNode>();
			OSMNode node_1 = new OSMNode(1254, 1.0, 2.0);
			OSMNode node_2 = new OSMNode(1255, 3.0, 4.0);

			target.Add(node_1);
			target.Add(node_2);

			target.RemoveAll();

			Assert.Equal(0, target.Count);
		}

		[Fact()]
		public void OSMObjectCollectionIntIndexerReturnsCorrectObjectByID() {
			OSMObjectCollection<OSMNode> target = new OSMObjectCollection<OSMNode>();
			OSMNode toAdd = new OSMNode(1254, 1.0, 2.0);

			target.Add(toAdd);

			Assert.Same(toAdd, target[toAdd.ID]);
		}

		[Fact()]
		public void OSMObjectCollectionIntIndexerThrowsExceptionIfIDIsNotPresent() {
			OSMObjectCollection<OSMNode> target = new OSMObjectCollection<OSMNode>();
			OSMNode toAdd = new OSMNode(1254, 1.0, 2.0);

			target.Add(toAdd);

            Assert.Throws<ArgumentException>(delegate { OSMNode result = target[1255]; });
		}

		[Fact()]
		public void OSMObjectCollectionImplementIEnumerable() {
			OSMObjectCollection<OSMNode> target = new OSMObjectCollection<OSMNode>();
			OSMNode node_1 = new OSMNode(1254, 1.0, 2.0);
			OSMNode node_2 = new OSMNode(1255, 3.0, 4.0);

			target.Add(node_1);
			target.Add(node_2);


			int counter = 0;
			foreach (OSMNode node in target) {
				counter++;
			}

			Assert.Equal(2, target.Count);
		}
	}
}
