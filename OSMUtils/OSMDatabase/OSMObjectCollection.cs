using System;
using System.Collections.Generic;
using System.Text;

namespace OSMUtils.OSMDatabase {
	public class OSMObjectCollection<T> : IEnumerable<T>  where T : OSMObject {
		protected Dictionary<int, T> _storage;

		public OSMObjectCollection() {
			_storage = new Dictionary<int, T>();
		}

		/// <summary>
		/// Gets the T object with the specified ID.
		/// </summary>
		/// <param name="key">ID of the object to get.</param>
		/// <returns>The T object with the specific ID.</returns>
		/// <exception cref="ArgumentException"></exception>
		public T this[int id] {
			get {
				try {
					return _storage[id];
				}
				catch (KeyNotFoundException e) {
					throw new ArgumentException(String.Format("The object with ID {0} wasn't found in the collection", id));
				}
			}
		}

		/// <summary>
		/// Gets the number of T objects in the collection
		/// </summary>
		public int Count {
			get {
				return _storage.Count;
			}
		}


		/// <summary>
		/// Adds an T object into the collection
		/// </summary>
		/// <param name="toAdd">The T object to be added to the collection</param>
		public void Add(T item) {
			if (item == null) {
				throw new ArgumentNullException("Can not add null to the collection");
			}

			_storage.Add(item.ID, item);
		}

		/// <summary>
		/// Removes the specific T object from the collection
		/// </summary>
		/// <param name="toRemove">The T object to remove from the collection</param>
		/// <returns>true if the object was removed from the collection, otherwise false</returns>
		public bool Remove(T toRemove) {
			return _storage.Remove(toRemove.ID);
		}

		/// <summary>
		/// Removes all objects from the collection
		/// </summary>
		public void RemoveAll() {
			_storage.Clear();
		}

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator() {
			return _storage.Values.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return _storage.Values.GetEnumerator();
		}

		#endregion
	}
}
