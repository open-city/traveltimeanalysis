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
	}
}
