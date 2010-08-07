//  Travel Time Analysis project
//  Copyright (C) 2010 Lukas Kabrt
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
		List<GPXRoute> _routes;

		public GPXXmlDataReaderTest() {
			_tracks = new List<GPXTrack>();
			_waypoints = new List<GPXPoint>();
			_routes = new List<GPXRoute>();
		}

		void Clear() {
			_tracks.Clear();
			_waypoints.Clear();
			_routes.Clear();
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

		[Fact()]
		public void GPXXmlDataReaderCanReadSimpleRoute() {
			Clear();

			GPXXmlDataReader target = new GPXXmlDataReader();
			target.RouteRead +=new GPXRouteReadHandler(ProcessRoute);

			// <rte>
			//   <rtept lat="50.0405788" lon="14.4694233" />
			//   <rtept lat="49.9116083" lon="14.7193313">
			//   </rtept>
			// </rte>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_simple_route));

			Assert.Equal(1, _routes.Count);
			Assert.Equal(2, _routes[0].NodesCount);
		}

		[Fact()]
		public void GPXXmlDataReaderCanReadNamedRoute() {
			Clear();

			GPXXmlDataReader target = new GPXXmlDataReader();
			target.RouteRead += new GPXRouteReadHandler(ProcessRoute);

			// <rte>
			//   <name>ROUTE NAME</name>
			//   <rtept lat="50.0405788" lon="14.4694233">
			//   </rtept>
			//   <rtept lat="49.9116083" lon="14.7193313">
			//   </rtept>
			//   <rtept lat="49.8448186" lon="14.7991063">
			//   </rtept>
			// </rte>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_named_route));

			Assert.Equal(1, _routes.Count);
			Assert.Equal("ROUTE NAME", _routes[0].Name);
			Assert.Equal(3, _routes[0].NodesCount);
		}

		public void GPXXmlDataReaderCanMultipleRoutes() {
			Clear();

			GPXXmlDataReader target = new GPXXmlDataReader();
			target.RouteRead += new GPXRouteReadHandler(ProcessRoute);

			//<rte>
			//  <name>TRACK 001</name>
			//  <rtept lat="50.0405788" lon="14.4694233">
			//  </rtept>
			//  <rtept lat="49.9116083" lon="14.7193313">
			//  </rtept>
			//  <rtept lat="49.8448186" lon="14.7991063">
			//  </rtept>
			//</rte>
  
			//<rte>
			//  <name>TRACK 002</name>
			//  <rtept lat="50.0405788" lon="14.4694233">
			//  </rtept>
			//  <rtept lat="49.9116083" lon="14.7193313">
			//  </rtept>
			//</rte>

			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_multiple_routes));

			Assert.Equal(2, _routes.Count);

			Assert.Equal("TRACK 001", _routes[0].Name);
			Assert.Equal(3, _routes[0].NodesCount);

			Assert.Equal("TRACK 002", _routes[1].Name);
			Assert.Equal(2, _routes[1].NodesCount);
		}

		[Fact()]
		public void GPXXmlDataReaderCanReadRealGPXFile() {
			Clear();

			GPXXmlDataReader target = new GPXXmlDataReader();
			target.WaypointRead += new GPXWaypointReadHandler(ProcessWaypoint);
			target.TrackRead += new GPXTrackReadHandler(ProcessTrack);
			target.RouteRead += new GPXRouteReadHandler(ProcessRoute);

			// 3 waypoints
			// 2 routes (3 and 1) points
			// 2 tracks
			//  1 segment, 2 points
			//  2 segments, 998 points and 10 points
			target.Read(new MemoryStream(GPXUtils.Tests.TestData.gpx_real));

			Assert.Equal(3, _waypoints.Count);

			Assert.Equal(2, _routes.Count);
			Assert.Equal(3, _routes[0].NodesCount);
			Assert.Equal(1, _routes[1].NodesCount);

			Assert.Equal(2, _tracks.Count);
			
			Assert.Equal(1, _tracks[0].Segments.Count);
			Assert.Equal(2, _tracks[0].Segments[0].NodesCount);

			Assert.Equal(2, _tracks[1].Segments.Count);
			Assert.Equal(998, _tracks[1].Segments[0].NodesCount);
			Assert.Equal(10, _tracks[1].Segments[1].NodesCount);
		}

		void ProcessWaypoint(GPXPoint waypoint) {
			_waypoints.Add(waypoint);
		}

		void ProcessTrack(GPXTrack track) {
			_tracks.Add(track);
		}

		void ProcessRoute(GPXRoute route) {
			_routes.Add(route);
		}
	}
}
