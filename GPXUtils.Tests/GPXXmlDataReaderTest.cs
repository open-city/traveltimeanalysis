using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Xunit;

using LK.GPXUtils;
using LK.GPXUtils.GPXDataSource;

namespace GPXUtils.Tests {
	public class GPXXmlDataReaderTest {
		List<GPXTrack> _tracks;
		List<GPXPoint> _waypoints;

		public GPXXmlDataReaderTest() {
			_tracks = new List<GPXTrack>();
			_waypoints = new List<GPXPoint>();
		}

		void Clear() {
			_tracks.Clear();
			_waypoints.Clear();
		}

		[Fact()]
		public void GPXXmlDataReaderReadThrowsExceptionIfFileDoesnotExist() {
			GPXXmlDataReader target = new GPXXmlDataReader();
			Assert.Throws<FileNotFoundException>(delegate { target.Read("non-existing-file.osm"); });
		}

		[Fact()]
		public void GPXXmlDataReaderReadThrowsExceptionReadingInvalidRootElement() {
			GPXXmlDataReader target = new GPXXmlDataReader();
			Assert.Throws<XmlException>(delegate { target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_invalid_root_element)); });
		}

		[Fact()]
		public void GPXXmlDataReaderCanReadSimpleWaypoint() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);

			//<wpt lat="50.4522196" lon="16.1117968">
			//</wpt>
			//<wpt lat="-50.4551789" lon="-16.1174158" />

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_simple_waypoints));

			Assert.Equal(2, _waypoints.Count);

			Assert.Equal(50.4522196, _waypoints[0].Latitude);
			Assert.Equal(16.1117968, _waypoints[0].Longitude);

			Assert.Equal(-50.4551789, _waypoints[1].Latitude);
			Assert.Equal(-16.1174158, _waypoints[1].Longitude);
		}

		[Fact()]
		public void GPXXmlDataReaderReadWaypointWithMissingLatitudeThrowsException() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);

			// <wpt lon="-16.1174158" />

			Assert.Throws<XmlException>(delegate { target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_simple_waypoint_missing_lat)); });
		}

		[Fact()]
		public void GPXXmlDataReaderReadWaypointWithMissingLongitudeThrowsException() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);

			// <wpt lat="-50.4551789" />

			Assert.Throws<XmlException>(delegate { target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_simple_waypoint_missing_lon)); });
		}


		[Fact()]
		public void GPXXmlDataReaderReadWaypointWithIncorrectLatitudeThrowsException() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);

			// <wpt lat="ERROR" lon="-16.1174158" />

			Assert.Throws<FormatException>(delegate { target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_simple_waypoint_incorrect_lat)); });
		}

		[Fact()]
		public void GPXXmlDataReaderReadWaypointWithIncorrectLongitudeThrowsException() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);

			// <wpt lat="-50.4551789" lon="ERROR" />

			Assert.Throws<FormatException>(delegate { target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_simple_waypoint_incorrect_lon)); });
		}

		[Fact()]
		public void GPXXmlDataReaderCanReadPointsElevtion() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);

			// <wpt lat="50.4522196" lon="16.1117968">
			//   <ele>433.4368896</ele>
			// </wpt>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_waypoint_with_elevation));

			Assert.Equal(1, _waypoints.Count);

			Assert.Equal(50.4522196, _waypoints[0].Latitude);
			Assert.Equal(16.1117968, _waypoints[0].Longitude);
			Assert.Equal(433.4368896, _waypoints[0].Elevation);
		}

		[Fact()]
		public void GPXXmlDataReaderCanReadPointsTime() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);

			// <wpt lat="50.4522196" lon="16.1117968">
			//   <time>2009-06-18T08:03:26Z</time>
			// </wpt>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_waypoint_with_time));

			Assert.Equal(1, _waypoints.Count);

			Assert.Equal(50.4522196, _waypoints[0].Latitude);
			Assert.Equal(16.1117968, _waypoints[0].Longitude);
			Assert.Equal(new DateTime(2009, 6, 18, 8, 3, 26, DateTimeKind.Utc), _waypoints[0].Time);
		}

		[Fact()]
		public void GPXXmlDataReaderCanReadPointsName() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);

			// <wpt lat="50.4522196" lon="16.1117968">
			//   <name>POINT 001</name>
			// </wpt>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_waypoint_with_name));

			Assert.Equal(1, _waypoints.Count);

			Assert.Equal(50.4522196, _waypoints[0].Latitude);
			Assert.Equal(16.1117968, _waypoints[0].Longitude);
			Assert.Equal("POINT 001", _waypoints[0].Name);
		}

		[Fact()]
		public void GPXXmlDataReaderCanReadPointsDescription() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);

			// <wpt lat="50.4522196" lon="16.1117968">
			//   <desc>DESCRIPTION</desc>
			// </wpt>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_waypoint_with_desc));

			Assert.Equal(1, _waypoints.Count);

			Assert.Equal(50.4522196, _waypoints[0].Latitude);
			Assert.Equal(16.1117968, _waypoints[0].Longitude);
			Assert.Equal("DESCRIPTION", _waypoints[0].Description);
		}

		[Fact()]
		public void GPXXmlDataReaderCanReadPointsComment() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);

			// <wpt lat="50.4522196" lon="16.1117968">
			//   <cmt>COMMENT</cmt>
			// </wpt>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_waypoint_with_comment));

			Assert.Equal(1, _waypoints.Count);

			Assert.Equal(50.4522196, _waypoints[0].Latitude);
			Assert.Equal(16.1117968, _waypoints[0].Longitude);
			Assert.Equal("COMMENT", _waypoints[0].Commenet);
		}

		[Fact()]
		public void GPXXmlDataReaderCanComplexPoint() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);

			// <wpt lat="50.4522196" lon="16.1117968">
			//   <cmt>COMMENT</cmt>
			//   <desc>DESCRIPTION</desc>
			//   <ele>433.4368896</ele>
			//   <name>POINT 001</name>
			//   <time>2009-06-18T08:03:26Z</time>
			// </wpt>
			// <wpt lat="50.4522196" lon="16.1117968">
			//   <name>POINT 002</name>
			// </wpt>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_complex_waypoint));

			Assert.Equal(2, _waypoints.Count);

			Assert.Equal(50.4522196, _waypoints[0].Latitude);
			Assert.Equal(16.1117968, _waypoints[0].Longitude);
			Assert.Equal(433.4368896, _waypoints[0].Elevation);
			Assert.Equal("DESCRIPTION", _waypoints[0].Description);
			Assert.Equal("COMMENT", _waypoints[0].Commenet);
			Assert.Equal("POINT 001", _waypoints[0].Name);
			Assert.Equal(new DateTime(2009, 6, 18, 8, 3, 26, DateTimeKind.Utc), _waypoints[0].Time);

			Assert.Equal("POINT 002", _waypoints[1].Name);
		}

		[Fact()]
		public void GPXXmlDataReaderCanReadSimpleTrack() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.TrackRead +=new GPXTrackReadHandler(ProcessTrack);

			// <trk>
			//   <trkseg>
			//     <trkpt lat="50.4950254" lon="16.1050424" />
			//     <trkpt lat="50.49503" lon="16.10503" />
			//   </trkseg>
			// </trk>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_simple_track));

			Assert.Equal(1, _tracks.Count);
			Assert.Equal(1, _tracks[0].Segments.Count);
			Assert.Equal(2, _tracks[0].Segments[0].NodesCount);
		}

		[Fact()]
		public void GPXXmlDataReaderCanReadTrackWithName() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.TrackRead += new GPXTrackReadHandler(ProcessTrack);

			// <trk>
			//   <name>TRACK NAME</name>
			//   <trkseg>
			//     <trkpt lat="50.4950254" lon="16.1050424" />
			//     <trkpt lat="50.49503" lon="16.10503" />
			//   </trkseg>
			// </trk>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_simple_named_track));

			Assert.Equal(1, _tracks.Count);

			Assert.Equal("TRACK NAME", _tracks[0].Name);
			Assert.Equal(1, _tracks[0].Segments.Count);
			Assert.Equal(2, _tracks[0].Segments[0].NodesCount);
		}

		[Fact()]
		public void GPXXmlDataReaderCanReadTrackWithMultipleSegments() {
			Clear();
			GPXXmlDataReader target = new GPXXmlDataReader();
			target.TrackRead += new GPXTrackReadHandler(ProcessTrack);

			// <trk>
			//   <trkseg>
			//     <trkpt lat="50.4950254" lon="16.1050424" />
			//     <trkpt lat="50.4950254" lon="16.1050424" />
			//   </trkseg>
			//   <trkseg>
			//     <trkpt lat="50.4950254" lon="16.1050424" />
			//     <trkpt lat="50.4950254" lon="16.1050424" />
			//     <trkpt lat="50.4950254" lon="16.1050424" />
			//   </trkseg>
			// </trk>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_track_multiple_segments));

			Assert.Equal(1, _tracks.Count);

			Assert.Equal(2, _tracks[0].Segments.Count);

			Assert.Equal(2, _tracks[0].Segments[0].NodesCount);
			Assert.Equal(3, _tracks[0].Segments[1].NodesCount);
		}

		void ProcessWaypoint(GPXPoint waypoint) {
			_waypoints.Add(waypoint);
		}

		void ProcessTrack(GPXTrack track) {
			_tracks.Add(track);
		}
	}
}
