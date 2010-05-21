using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.IO;

namespace LK.GPXUtils.GPXDataSource {
	public class GPXXmlDataWriter : IGPXDataWriter, IDisposable {
		private bool _closed;
		protected XmlWriter _xmlWriter;

		/// <summary>
		/// Creates a new GPXXmlDataWriter, which writes GPX data into the specific file.
		/// </summary>
		/// <param name="filename">Path to the file</param>
		public GPXXmlDataWriter(string filename) {
			XmlWriterSettings writerSetting = new XmlWriterSettings();
			writerSetting.Indent = true;

			_xmlWriter = XmlTextWriter.Create(new StreamWriter(filename, false, new UTF8Encoding(false)), writerSetting);

			WriteGPXElement();
		}

		/// <summary>
		/// Creates a new OXMXmlDataWriter, which writes OSM entities into the specific stream.
		/// </summary>
		/// <param name="stream"></param>
		public GPXXmlDataWriter(Stream stream) {
			XmlWriterSettings writerSetting = new XmlWriterSettings();
			writerSetting.Indent = true;

			_xmlWriter = XmlTextWriter.Create(new StreamWriter(stream, new UTF8Encoding(false)), writerSetting);

			WriteGPXElement();
		}

		/// <summary>
		/// Closes GPXXmlDataWrites, no more elements can be written after calling this method.
		/// </summary>
		public void Close() {
			_xmlWriter.WriteEndElement();
			_xmlWriter.Close();
			_closed = true;
		}

		private void WriteGPXElement() {
			_xmlWriter.WriteStartElement("gpx");
			_xmlWriter.WriteAttributeString("version", "1.1");
			_xmlWriter.WriteAttributeString("creator", "GPXUtils");
		}

		public void WriteWaypoint(GPXPoint waypoint) {
			_xmlWriter.WriteStartElement("wpt");
			WritePointData(waypoint);
			_xmlWriter.WriteEndElement();
		}

		protected void WritePointData(GPXPoint point) {
			_xmlWriter.WriteAttributeString("lat", point.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture));
			_xmlWriter.WriteAttributeString("lon", point.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture));

			if (point.Elevation != 0) {
				_xmlWriter.WriteElementString("ele", point.Elevation.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}

			if (point.Time != DateTime.MinValue) {
				_xmlWriter.WriteElementString("time", point.Time.ToString(System.Globalization.CultureInfo.InvariantCulture));			
			}

			if (string.IsNullOrEmpty(point.Name) == false) {
				_xmlWriter.WriteElementString("name", point.Name);
			}

			if (string.IsNullOrEmpty(point.Description) == false) {
				_xmlWriter.WriteElementString("desc", point.Description);
			}

			if (string.IsNullOrEmpty(point.Commenet) == false) {
				_xmlWriter.WriteElementString("cmt", point.Commenet);
			}
		}

		public void WriteRoute(GPXRoute route) {
			_xmlWriter.WriteStartElement("rte");

			if (string.IsNullOrEmpty(route.Name) == false) {
				_xmlWriter.WriteElementString("name", route.Name);
			}

			foreach (GPXPoint point in route.Nodes) {
				_xmlWriter.WriteStartElement("rtept");
				WritePointData(point);
				_xmlWriter.WriteEndElement();
			}
			_xmlWriter.WriteEndElement();
		}

		public void WriteTrack(GPXTrack track) {
			throw new NotImplementedException();
		}

		#region IDisposable Members

		private bool _disposed = false;
		/// <summary>
		/// Implements IDisposable interface
		/// </summary>
		public void Dispose() {
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Closes OSMXmlDataWriter on Disposing
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing) {
			if (!this._disposed) {
				if (_closed == false) {
					Close();
				}

				_disposed = true;
			}
		}

		#endregion
	}
}
