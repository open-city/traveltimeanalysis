using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.MatchGPX2OSM {
	/// <summary>
	/// Represents partial Path in the AStar pathfinder
	/// </summary>
	class PartialPath : IComparer<PartialPath>, IComparable {
		public Node CurrentPosition;
		public Node PreviousNode;
		public ConnectionGeometry PreviousPath;
		public double Length;

		/// <summary>
		/// Determines whether Obj equals this PartialPath
		/// </summary>
		/// <param name="obj">The object to compare</param>
		/// <returns>true if Obj is PartialPath and CurrentPosition are the same, otherwise false</returns>
		public override bool Equals(object obj) {
			PartialPath other = obj as PartialPath;
			if (other != null) {
				return this.CurrentPosition.Equals(other.CurrentPosition);
			}
			else
				return false;
		}

		/// <summary>
		/// Determines whether Other equals this PartialPath
		/// </summary>
		/// <param name="other">The PartialPath to compare with the current PartialPath</param>
		/// <returns>true if CurrentPosition are the same, otherwise false</returns>
		public bool Equals(PartialPath other) {
				return this.CurrentPosition.Equals(other.CurrentPosition);
		}

		/// <summary>
		/// Returns a hash code for the current PartialPath
		/// </summary>
		/// <returns>A hash code for the current Segment.</returns>
		public override int GetHashCode() {
			return this.CurrentPosition.GetHashCode();
		}

		#region IComparer<Path> Members

		public int Compare(PartialPath x, PartialPath y) {
			return x.Length.CompareTo(y.Length);
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj) {
			if (obj is PartialPath) {
				PartialPath other = (PartialPath)obj;
				return Compare(this, other);
			}
			return 0;
		}

		#endregion
	}
}
