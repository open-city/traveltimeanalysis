using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.GPXUtils {
	/// <summary>
	/// Represents a track
	/// </summary>
	public class GPXTrack {
		/// <summary>
		/// Gets or sets the name of this track
		/// </summary>
		public string Name {
			get;
			set;
		}

		protected List<GPXTrackSegment> _segments;

		/// <summary>
		/// Gets the list of track segments that are part of this track
		/// </summary>
		public List<GPXTrackSegment> Segments {
			get {
				return _segments;
			}
		}

		/// <summary>
		/// Creates a new, empty instance of GPXTrack 
		/// </summary>
		public GPXTrack() {
			_segments = new List<GPXTrackSegment>();
		}

		/// <summary>
		/// Creates a new instance of GPXTrack with specific name
		/// </summary>
		/// <param name="name">The name of this GPXTrack</param>
		public GPXTrack(string name) {
			_segments = new List<GPXTrackSegment>();

			Name = name;
		}
	}
}
