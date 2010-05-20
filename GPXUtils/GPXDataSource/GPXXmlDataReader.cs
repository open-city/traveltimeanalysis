using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using LK.GPXUtils;

namespace LK.GPXUtils.GPXDataSource {
	/// <summary>
	/// Provides minimalistic implementation of IGPXDataReader
	/// </summary>
	/// <remarks>GPXXmlDataReader can process only tracks</remarks>
	public class GPXXmlDataReader : IGPXDataReader {
		XmlReader _xmlReader;

		/// <summary>
		/// Reads data from the gpx file
		/// </summary>
		/// <param name="osmFile">Path to the gpx file.</param>
		public void Read(string gpxFile) {
			using (FileStream fs = new FileStream(gpxFile, FileMode.Open)) {
				this.Read(fs);
			}
		}

		/// <summary>
		/// Reads data from a stream
		/// </summary>
		/// <param name="stream">The stram to read data from</param>
		public void Read(Stream stream) {

			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			xmlReaderSettings.IgnoreWhitespace = true;

			try {
				_xmlReader = XmlTextReader.Create(stream, xmlReaderSettings);

				_xmlReader.Read();
				while (false == _xmlReader.EOF) {


					switch (_xmlReader.NodeType) {
						case XmlNodeType.XmlDeclaration:
							_xmlReader.Read();
							continue;

						case XmlNodeType.Element:
							if (_xmlReader.Name != "gpx")
								throw new XmlException("Invalid xml root element. Expected <gpx>.");

							ReadGPXTag();
							return;

						default:
							throw new XmlException();
					}
				}
			}
			finally {
				_xmlReader.Close();
				_xmlReader = null;
			}
		}

		/// <summary>
		/// Reads content of the root gpx element
		/// </summary>
		private void ReadGPXTag() {
			_xmlReader.Read();

			while (_xmlReader.NodeType != XmlNodeType.EndElement) {
				switch (_xmlReader.Name) {
					case "wpt":
						ReadPoint();
						break;
					//case "relation":
					//  ReadRelation();
					//  break;
					//case "way":
					//  ReadWay();
					//  break;
					default:
						_xmlReader.Skip();
						break;
				}
			}
		}

		/// <summary>
		/// Reads a point from gpx document
		/// </summary>
		private void ReadPoint() {
			// latitude attribute
			string lat = _xmlReader.GetAttribute("lat");
			
			if (string.IsNullOrEmpty(lat)) {
				throw new XmlException("Attribute 'lat' is missing.");
			}
			double pointLat = double.Parse(lat, System.Globalization.CultureInfo.InvariantCulture);

			// longitude attribute
			string lon = _xmlReader.GetAttribute("lon");

			if (string.IsNullOrEmpty(lon)) {
				throw new XmlException("Attribute 'lon' is missing.");
			}
			double pointLon = double.Parse(lon, System.Globalization.CultureInfo.InvariantCulture);

			GPXPoint parsedPoint = new GPXPoint(pointLat, pointLon);

			if (_xmlReader.IsEmptyElement == false) {
				_xmlReader.Read();

				while (_xmlReader.NodeType != XmlNodeType.EndElement) {
					if (_xmlReader.NodeType == XmlNodeType.Element) {
						switch(_xmlReader.Name) {
							case "ele":
								string ele = _xmlReader.ReadString();
								parsedPoint.Elevation = double.Parse(ele, System.Globalization.CultureInfo.InvariantCulture);
								break;
							case "time":
								string time = _xmlReader.ReadString();
								parsedPoint.Time = DateTime.Parse(time, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
								break;
							case "name":
								parsedPoint.Name = _xmlReader.ReadString();
								break;
							case "desc":
								parsedPoint.Description = _xmlReader.ReadString();
								break;
							case "cmt":
								parsedPoint.Commenet = _xmlReader.ReadString();
								break;
							default:
								_xmlReader.Skip();
								break;
						}
					}
					else {
						_xmlReader.Skip();
					}
				}
			}

			OnWaypointRead(parsedPoint);
			_xmlReader.Skip();
		}
		
		#region event handling

		/// <summary>
		/// Occurs when a track is read from xml
		/// </summary>
		public event GPXTrackReadHandler TrackRead;

		/// <summary>
		/// Raises the TrackRead event
		/// </summary>
		/// <param name="node">The track read from the xml</param>
		protected void OnTrackRead(GPXTrack track) {
			GPXTrackReadHandler temp = TrackRead;
			if (temp != null) {
				temp(track);
			}
		}

		/// <summary>
		/// Occurs when a route is read from xml
		/// </summary>
		public event GPXTrackReadHandler RouteRead;

		/// <summary>
		/// Raises the RouteRead event
		/// </summary>
		/// <param name="node">The route read from the xml</param>
		protected void OnRouteRead(GPXTrack route) {
			GPXTrackReadHandler temp = RouteRead;
			if (temp != null) {
				temp(route);
			}
		}

		/// <summary>
		/// Occurs when a waypoint is read from xml
		/// </summary>
		public event GPXWaypointReadHandler WaypointRead;

		/// <summary>
		/// Raises the WaypointRead event
		/// </summary>
		/// <param name="node">The waypoint read from the xml</param>
		protected void OnWaypointRead(GPXPoint waypoint) {
			GPXWaypointReadHandler temp = WaypointRead;
			if (temp != null) {
				temp(waypoint);
			}
		}

		#endregion
	}
}
