using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.CommonLib.Collections;

namespace LK.MatchGPX2OSM {
	/// <summary>
	/// Represents a priority queue of the PartialPath, it is used as open list in the A* algorithm
	/// </summary>
	class PartialPathList : BinaryHeap<PartialPath> {
		Dictionary<Node, PartialPath> _paths;

		/// <summary>
		/// Creates a new instance of the PartialPathList
		/// </summary>
		public PartialPathList()
			: base() {
				_paths = new Dictionary<Node, PartialPath>();
		}

		/// <summary>
		/// Gets the PartialPath with the specified Position
		/// </summary>
		/// <param name="position">The Position of PartialPath</param>
		/// <returns></returns>
		public PartialPath this[Node position] {
			get {
				return _paths[position];
			}
		}

		/// <summary>
		/// Updates the PartialPath length
		/// </summary>
		/// <param name="item">The item to update</param>
		/// <param name="pathLength">The new length of the PartialPath</param>
		public void Update(PartialPath item, double pathLength) {
			base.Remove(item);

			item.Length = pathLength;
			base.Add(item);
		}
		
		/// <summary>
		/// Adds a new item to the List
		/// </summary>
		/// <param name="item">The item to add</param>
		public new void Add(PartialPath item) {
			_paths.Add(item.End, item);
			base.Add(item);
		}

		/// <summary>
		/// Removes the item from the List
		/// </summary>
		/// <param name="item">The item to remove</param>
		/// <returns>true if item was removed, otherwise returns false</returns>
		public new bool Remove(PartialPath item) {
			_paths.Remove(item.End);
			return base.Remove(item);
		}

		/// <summary>
		/// Removes the shortest PartialPath from the list
		/// </summary>
		/// <returns>The shortest PartialPath object from the list</returns>
		public new PartialPath RemoveTop() {
			PartialPath result = base.RemoveTop();
			_paths.Remove(result.End);

			return result;
		}

		/// <summary>
		/// Determinates whether List contains the specified item
		/// </summary>
		/// <param name="item">The item to check</param>
		/// <returns>true if item is presented in the List, otherwise returns false</returns>
		public new bool Contains(PartialPath item) {
			return _paths.ContainsKey(item.End);
		}

		/// <summary>
		/// Determinates whether List contains the item with the specified Position
		/// </summary>
		/// <param name="item">The Position of the item</param>
		/// <returns>true if item is presented in the List, otherwise returns false</returns>
		public bool Contains(Node position) {
			return _paths.ContainsKey(position);
		}

		/// <summary>
		/// Removes all objects from the List
		/// </summary>
		public new void Clear() {
			base.Clear();
			_paths.Clear();
		}
	}
}
