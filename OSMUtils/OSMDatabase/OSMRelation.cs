using System;
using System.Collections.Generic;
using System.Text;

namespace LK.OSMUtils.OSMDatabase {
	public class OSMRelation : OSMObject {
		/// <summary>
		/// Creates a new OSMRelation with the scpecific ID.
		/// </summary>
		/// <param name="id">ID of the OSMRelation.</param>
		public OSMRelation(int id)
			: base(id) {
				_members = new List<OSMRelationMember>();
		}

		protected List<OSMRelationMember> _members;
		/// <summary>
		/// Gets list of node IDs, that forms the way
		/// </summary>
		public IList<OSMRelationMember> Members {
			get {
				return _members;
			}
		}
	}
}
