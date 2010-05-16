using System;
using System.Collections.Generic;
using System.Text;

namespace OSMUtils.OSMDatabase {
	public abstract class OSMObject {
		private int _id;
		/// <summary>
		/// Gets or sets object ID
		/// </summary>
		public int ID {
			get {
				return _id;
			}
		}

		/// <summary>
		/// Intializes a new OSM object with the specified ID 
		/// </summary>
		/// <param name="id">Object ID.</param>
		protected OSMObject(int id) {
			_id = id;
			_tags = new OSMTagsCollection();
		}

		protected OSMTagsCollection _tags;
		/// <summary>
		/// Gets the collection of tags asociated with the current obejct
		/// </summary>
		public OSMTagsCollection Tags {
			get {
				return _tags;
			}
		}

		/// <summary>
		/// Returns a string representation the currenet object
		/// </summary>
		/// <returns>A string that represents the current object</returns>
		public override string ToString() {
			return _id.ToString();
		}

		/// <summary>
		/// Compares the current OSMObject with the specified object for equivalence.
		/// </summary>
		/// <param name="obj">The object to test for equivalence with the current object.</param>
		/// <returns>true if the OSMTag objects are equal, otherwise returns false.</returns>
		public override bool Equals(object obj) {
			if (obj == null)
				return false;

			if (obj is OSMObject) {
				OSMObject other = (OSMObject)obj;

				return _id.Equals(other._id);
			}
			else
				return base.Equals(obj);
		}

		/// <summary>
		/// Returns the hash code for the current object
		/// </summary>
		/// <returns>An integer hash code</returns>
		public override int GetHashCode() {
			return _id.GetHashCode();
		}


		//private DateTime _timestamp;
		///// <summary>
		///// Gets or sets timestamp 
		///// </summary>
		//public DateTime Timestamp {
		//  get { return _timestamp; }
		//  set { _timestamp = value; }
		//}


		//private int _version;

		//public int Version {
		//  get { return _version; }
		//}


		//private bool _visible;

		//public bool Visible {
		//  get { return _visible; }
		//  set { _visible = value; }
		//}

		//private string _user;

		//public string User {
		//  get { return _user; }
		//}

		//private int _userID;

		//public int UserID {
		//  get { return _userID; }
		//}

		//private int _changeset;

		//public int Changeset {
		//  get { return _changeset; }
		//}


	}
}
