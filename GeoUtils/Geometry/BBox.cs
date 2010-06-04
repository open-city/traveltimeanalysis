using System;
using System.Collections.Generic;
using System.Text;

namespace LK.GeoUtils.Geometry {
	/// <summary>
	/// Represents the bounding box of the group of points
	/// </summary>
	public class BBox {
		private double _north;

		public double North {
			get {	return _north; }
			set {	_north = value; }
			}

		private double _south;

		public double South {
			get { return _south; }
			set { _south = value; }
		}

		private double _east;

		public double East {
			get { return _east; }
			set { _east = value; }
		}

		private double _west;

		public double West {
			get { return _west; }
			set { _west = value; }
		}


		private double _minElevation;

		public double MinElevation {
			get { return _minElevation; }
			set { _minElevation = value; }
		}

		private double _maxElevation;

		public double MaxElevation {
			get { return _maxElevation; }
			set { _maxElevation = value; }
		}

		protected bool initialized;

		/// <summary>
		/// Creates a new BBox
		/// </summary>
		public BBox() {
			initialized = false;
		}

		/// <summary>
		/// Creates a nex BBox for the collection of points
		/// </summary>
		/// <param name="pointsToCover">The collection of points to be covered by this BBox</param>
		public BBox(IEnumerable<IPointGeo> pointsToCover) {
			foreach (IPointGeo pointToCover in pointsToCover) {
				ExtendToCover(pointToCover);
			}
		}

		/// <summary>
		/// Extends the BBox to cover the given IPointGeo
		/// </summary>
		/// <param name="toCover">The point to be covered by this BBox</param>
		public void ExtendToCover(IPointGeo toCover) {
			if (initialized == false) {
				_north = toCover.Latitude;
				_south = toCover.Latitude;
				_east = toCover.Longitude;
				_west = toCover.Longitude;

				_minElevation = toCover.Elevation;
				_maxElevation = toCover.Elevation;

				initialized = true;
			}

			_north = Math.Max(_north, toCover.Latitude);
			_south = Math.Min(_south, toCover.Latitude);
			_east = Math.Max(_east, toCover.Longitude);
			_west = Math.Min(_west, toCover.Longitude);

			_minElevation = Math.Min(_minElevation, toCover.Elevation);
			_maxElevation = Math.Max(_maxElevation, toCover.Elevation);
		}

		/// <summary>
		/// Tests if the specific point is inside this BBox
		/// </summary>
		/// <param name="point">The point to be tested</param>
		/// <returns>True if the point is inside this BBox or on the boundary of this BBox otherwise returns false</returns>
		public bool IsInside(IPointGeo point) {
			return point.Latitude <= _north && point.Latitude >= _south &&
						 point.Longitude <= _east && point.Longitude >= _west &&
						 point.Elevation <= _maxElevation && point.Elevation >= _minElevation;
		}

		/// <summary>
		/// Inflates bbox around it's center
		/// </summary>
		/// <param name="deg">Number of degrees</param>
		public void Inflate(double deg) {
			this.initialized = true;

			_south -= deg;
			_north += deg;
			_east += deg;
			_west -= deg;
		}

		public PointGeo[] Corners {
			get {
				return new PointGeo[] {new PointGeo(North, West), new PointGeo(North, East),
															 new PointGeo(South, East), new PointGeo(South, West)};
			}
		}
	}
}
