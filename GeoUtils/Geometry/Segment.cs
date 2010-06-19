using System;
using System.Collections.Generic;
using System.Text;

namespace LK.GeoUtils.Geometry {
	/// <summary>
	/// Represents part of GPS trace between two points
	/// </summary>
	public class Segment<T> where T : IPointGeo {
		/// <summary>
		/// Gets the start point of this Segment
		/// </summary>
		public T StartPoint { get; private set; }
		
		/// <summary>
		/// Gets the end point of this Segment
		/// </summary>
		public T EndPoint { get; private set; }

		/// <summary>
		/// Creates a new segment with the specific start and end points
		/// </summary>
		/// <param name="start">The point, where segment starts</param>
		/// <param name="end">The point, where segment ends</param>
		public Segment(T start, T end)  {
			StartPoint = start;
			EndPoint = end;

			_length = Calculations.GetDistance2D(StartPoint, EndPoint);
		}

		private double _length;
		/// <summary>
		/// Gets length of the segment in meters
		/// </summary>
		public double Length {
			get {
				return _length;
			}
		}

		/// <summary>
		/// Determines whethet specified object is equal to the current Segment
		/// </summary>
		/// <param name="obj">The object to compare with the current Segment</param>
		/// <returns>true if obj is Segment and has the same StartPoint and EndPoint as this Segment, otherwise returns false</returns>
		public override bool Equals(object obj) {
			Segment<T> other = obj as Segment<T>;
			if (other != null) {
				return StartPoint.Equals(other.StartPoint) && EndPoint.Equals(other.EndPoint);
			}
			else
				return false;
		}

		/// <summary>
		/// Determines whethet specified object is equal to the current Segment
		/// </summary>
		/// <param name="obj">The object to compare with the current Segment</param>
		/// <returns>true if obj is Segment and has the same StartPoint and EndPoint as this Segment, otherwise returns false</returns>
		public bool Equals(Segment<T> other) {
			if (other != null) {
				return StartPoint.Equals(other.StartPoint) && EndPoint.Equals(other.EndPoint);
			}
			else
				return false;
		}

		/// <summary>
		/// Returns a hash code for the current Segment
		/// </summary>
		/// <returns>A hash code for the current Segment.</returns>
		public override int GetHashCode() {
			return unchecked(StartPoint.GetHashCode() + 31 * EndPoint.GetHashCode());
		}
	}
}
