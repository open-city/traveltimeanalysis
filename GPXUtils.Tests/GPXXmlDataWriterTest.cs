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
using System.Xml.Linq;

using Xunit;

using LK.GPXUtils;
using LK.GPXUtils.GPXDataSource;

namespace GPXUtils.Tests {
	public class GPXXmlDataWriterTest {
		[Fact()]
		public void GPXXmlDataWriterConstructorCreatesEmptyFile() {
			MemoryStream ms = new MemoryStream();

			using (GPXXmlDataWriter target = new GPXXmlDataWriter(ms)) {
			}

			ms.Seek(0, 0);
			XElement osmRoot = XDocument.Load(new StreamReader(ms)).Root;

			Assert.Equal("gpx", osmRoot.Name);
			Assert.False(osmRoot.HasElements);
		}

		[Fact()]
		public void GPXXmlDataWriterWriteWaypointWritesWaypointWithAllAttributes() {
			MemoryStream ms = new MemoryStream();
			GPXPoint waypoint = new GPXPoint(18.5, 50.1);
			waypoint.Elevation = 1600;
			waypoint.Time = new DateTime(2009, 12, 31, 23, 50, 0, DateTimeKind.Utc);
			waypoint.Name = "NAME";
			waypoint.Description = "DESCRIPTION";
			waypoint.Commenet = "COMMENT";

			using (GPXXmlDataWriter target = new GPXXmlDataWriter(ms)) {
				target.WriteWaypoint(waypoint);
			}

			ms.Seek(0, 0);

			XElement gpxRoot = XDocument.Load(new StreamReader(ms)).Root;
			XElement waypointElement = gpxRoot.Element("wpt");

			Assert.NotNull(waypointElement);

			Assert.Equal(waypoint.Latitude, double.Parse(waypointElement.Attribute("lat").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(waypoint.Longitude, double.Parse(waypointElement.Attribute("lon").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(waypoint.Elevation, double.Parse(waypointElement.Element("ele").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(waypoint.Time, DateTime.Parse(waypointElement.Element("time").Value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal));
			Assert.Equal(waypoint.Name, waypointElement.Element("name").Value);
			Assert.Equal(waypoint.Description, waypointElement.Element("desc").Value);
			Assert.Equal(waypoint.Commenet, waypointElement.Element("cmt").Value);
		}

		[Fact()]
		public void GPXXmlDataWriterWriteRouteWritesRoutePointsWithAllAttributes() {
			MemoryStream ms = new MemoryStream();

			GPXRoute route = new GPXRoute();

			GPXPoint point = new GPXPoint(18.5, 50.1);
			point.Elevation = 1600;
			point.Time = new DateTime(2009, 12, 31, 23, 50, 0, DateTimeKind.Utc);
			point.Name = "NAME";
			point.Description = "DESCRIPTION";
			point.Commenet = "COMMENT";

			route.Nodes.Add(point);

			using (GPXXmlDataWriter target = new GPXXmlDataWriter(ms)) {
				target.WriteRoute(route);
			}

			ms.Seek(0, 0);

			XElement gpxRoot = XDocument.Load(new StreamReader(ms)).Root;
			XElement routeElement = gpxRoot.Element("rte");

			Assert.NotNull(routeElement);

			XElement pointElement = routeElement.Element("rtept");

			Assert.NotNull(pointElement);

			Assert.Equal(point.Latitude, double.Parse(pointElement.Attribute("lat").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(point.Longitude, double.Parse(pointElement.Attribute("lon").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(point.Elevation, double.Parse(pointElement.Element("ele").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(point.Time, DateTime.Parse(pointElement.Element("time").Value, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal));
			Assert.Equal(point.Name, pointElement.Element("name").Value);
			Assert.Equal(point.Description, pointElement.Element("desc").Value);
			Assert.Equal(point.Commenet, pointElement.Element("cmt").Value);
		}

		[Fact()]
		public void GPXXmlDataWriterWriteRouteWritesRoute() {
			MemoryStream ms = new MemoryStream();

			GPXRoute route = new GPXRoute("ROUTE NAME");
			GPXPoint point1 = new GPXPoint(18.5, 50.1);
			GPXPoint point2 = new GPXPoint(10.3, 20.5);

			route.Nodes.Add(point1);
			route.Nodes.Add(point2);

			using (GPXXmlDataWriter target = new GPXXmlDataWriter(ms)) {
				target.WriteRoute(route);
			}

			ms.Seek(0, 0);

			XElement gpxRoot = XDocument.Load(new StreamReader(ms)).Root;
			XElement routeElement = gpxRoot.Element("rte");

			Assert.NotNull(routeElement);

			Assert.Equal(route.Name, routeElement.Element("name").Value);
			
			var points = routeElement.Elements("rtept").ToList();
			Assert.Equal(2, points.Count);
			Assert.Equal(point1.Latitude, double.Parse(points[0].Attribute("lat").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(point1.Longitude, double.Parse(points[0].Attribute("lon").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(point2.Latitude, double.Parse(points[1].Attribute("lat").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(point2.Longitude, double.Parse(points[1].Attribute("lon").Value, System.Globalization.CultureInfo.InvariantCulture));
		}

		[Fact()]
		public void GPXXmlDataWriterWriteSimpleTrack() {
			MemoryStream ms = new MemoryStream();

			GPXTrack track = new GPXTrack();
			track.Name = "TRACK NAME";

			GPXPoint point1 = new GPXPoint(18.5, 50.1);
			GPXPoint point2 = new GPXPoint(10.3, 20.5);

			GPXTrackSegment segment = new GPXTrackSegment();
			segment.Nodes.Add(point1);
			segment.Nodes.Add(point2);

			track.Segments.Add(segment);


			using (GPXXmlDataWriter target = new GPXXmlDataWriter(ms)) {
				target.WriteTrack(track);
			}

			ms.Seek(0, 0);

			XElement gpxRoot = XDocument.Load(new StreamReader(ms)).Root;
			
			XElement trackElement = gpxRoot.Element("trk");
			Assert.NotNull(trackElement);
			Assert.Equal(track.Name, trackElement.Element("name").Value);

			XElement trackSegmentElement = trackElement.Element("trkseg");
			Assert.NotNull(trackSegmentElement);

			var points = trackSegmentElement.Elements("trkpt").ToList();
			Assert.Equal(2, points.Count);
			Assert.Equal(point1.Latitude, double.Parse(points[0].Attribute("lat").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(point1.Longitude, double.Parse(points[0].Attribute("lon").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(point2.Latitude, double.Parse(points[1].Attribute("lat").Value, System.Globalization.CultureInfo.InvariantCulture));
			Assert.Equal(point2.Longitude, double.Parse(points[1].Attribute("lon").Value, System.Globalization.CultureInfo.InvariantCulture));
		}

		[Fact()]
		public void GPXXmlDataWriterWriteTrackWithMultipleSegments() {
			MemoryStream ms = new MemoryStream();

			GPXTrack track = new GPXTrack();
			track.Name = "TRACK NAME";

			GPXPoint point1 = new GPXPoint(18.5, 50.1);
			GPXPoint point2 = new GPXPoint(10.3, 20.5);
			GPXPoint point3 = new GPXPoint(8.2, 28.8);

			GPXTrackSegment segment1 = new GPXTrackSegment();
			segment1.Nodes.Add(point1);
			segment1.Nodes.Add(point2);
			segment1.Nodes.Add(point3);
			track.Segments.Add(segment1);

			GPXTrackSegment segment2 = new GPXTrackSegment();
			segment2.Nodes.Add(point1);
			segment2.Nodes.Add(point2);
			track.Segments.Add(segment2);

			using (GPXXmlDataWriter target = new GPXXmlDataWriter(ms)) {
				target.WriteTrack(track);
			}

			ms.Seek(0, 0);

			XElement gpxRoot = XDocument.Load(new StreamReader(ms)).Root;

			XElement trackElement = gpxRoot.Element("trk");
			Assert.NotNull(trackElement);
			Assert.Equal(track.Name, trackElement.Element("name").Value);

			var trackSegments = trackElement.Elements("trkseg").ToList();
			Assert.Equal(track.Segments.Count, trackSegments.Count);

			var points = trackSegments[0].Elements("trkpt").ToList();
			Assert.Equal(track.Segments[0].NodesCount, points.Count);

			points = trackSegments[1].Elements("trkpt").ToList();
			Assert.Equal(track.Segments[1].NodesCount, points.Count);
		}
	}
}
