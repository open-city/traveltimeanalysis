using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.MatchGPX2OSM {
	/// <summary>
	/// Represents partial Path in the AStar pathfinder
	/// </summary>
	class PartialPath : IComparer<PartialPath>, IComparable {
		public Node End;
		public Node PreviousNode;
		public ConnectionGeometry PathFromPrevious;
		public double Length;
		public double EstimationToEnd;

		/// <summary>
		/// Determines whether Obj equals this PartialPath
		/// </summary>
		/// <param name="obj">The object to compare</param>
		/// <returns>true if Obj is PartialPath and CurrentPosition are the same, otherwise false</returns>
		public override bool Equals(object obj) {
			PartialPath other = obj as PartialPath;
			if (other != null) {
				return this.End.Equals(other.End);
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
				return this.End.Equals(other.End);
		}

		/// <summary>
		/// Returns a hash code for the current PartialPath
		/// </summary>
		/// <returns>A hash code for the current Segment.</returns>
		public override int GetHashCode() {
			return this.End.GetHashCode();
		}

		#region IComparer<Path> Members

		public int Compare(PartialPath x, PartialPath y) {
			double totalLength = x.Length + x.EstimationToEnd;

			return totalLength.CompareTo(y.Length + y.EstimationToEnd);
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
