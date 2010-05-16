using System;
using System.Collections.Generic;
using System.Text;

namespace OSMUtils.OSMDatabase {
	/// <summary>
	/// Represents a member of the OSM relation
	/// </summary>
	public class OSMRelationMember {
		private OSMRelationMemberType _type;
		/// <summary>
		/// Gets the type of the relation member
		/// </summary>
		public OSMRelationMemberType Type {
			get { return _type; }
		}

		private int _reference;
		/// <summary>
		/// Gets the reference of this relation member
		/// </summary>
		public int Reference {
			get { return _reference; }
		}

		private string _role;
		/// <summary>
		/// Gets or sets the role of the relation's member.
		/// </summary>
		public string Role {
			get { return _role; }
			set { _role = value; }
		}

		/// <summary>
		/// Creates a new OSMRelationMember object with the given value
		/// </summary>
		/// <param name="type">The type of this relation member</param>
		/// <param name="reference">The reference of this relation member </param>
		/// <param name="role">The role of this member. It can be null.</param>
		public OSMRelationMember(OSMRelationMemberType type, int reference, string role) {
			_type = type;
			_reference = reference;
			_role = role;
		}

	}
}
