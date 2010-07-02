using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using LK.GPXUtils;

namespace LK.Analyzer {
	public class XmlTravelTimeDB : ITravelTimesDB {
		Dictionary<SegmentInfo, List<TravelTime>> _storage;
		XmlReader _xmlReader;
		XmlWriter _xmlWriter;

		public IEnumerable<TravelTime> TravelTimes {
			get {
				return _storage.Values.SelectMany(segTravelTimes => segTravelTimes);
			}
		}

		public IEnumerable<TravelTime> GetTravelTimes(SegmentInfo segment) {
			return _storage[segment];
		}

		public void AddTravelTime(TravelTime toAdd) {
			if (_storage.ContainsKey(toAdd.Segment) == false) {
				_storage.Add(toAdd.Segment, new List<TravelTime>());
			}

			_storage[toAdd.Segment].Add(toAdd);
		}

		public bool RemoveTravelTime(TravelTime toRemove) {
			if (_storage.ContainsKey(toRemove.Segment)) {
				return _storage[toRemove.Segment].Remove(toRemove);
			}

			return false;
		}

		public XmlTravelTimeDB() {
			_storage = new Dictionary<SegmentInfo, List<TravelTime>>();
		}

		public void Save(string filename) {
			using (FileStream fs = new FileStream(filename, FileMode.Create)) {
				Save(fs);
			}
		}

		public void Save(Stream stream) {
			XmlWriterSettings writerSetting = new XmlWriterSettings();
			writerSetting.Indent = true;

			_xmlWriter = XmlTextWriter.Create(new StreamWriter(stream, new UTF8Encoding(false)), writerSetting);

			_xmlWriter.WriteStartElement("travel-time-db");

			foreach (var segment in _storage.Keys) {
				WriteSegment(segment);
			}

			_xmlWriter.WriteEndElement();
			_xmlWriter.Close();
		}

		void WriteSegment(SegmentInfo segment) {
			_xmlWriter.WriteStartElement("segment");
			_xmlWriter.WriteAttributeString("from", segment.NodeFromID.ToString());
			_xmlWriter.WriteAttributeString("to", segment.NodeToID.ToString());
			_xmlWriter.WriteAttributeString("way", segment.WayID.ToString());

			foreach (var traveltime in _storage[segment]) {
				WriteTravelTime(traveltime);
			}

			_xmlWriter.WriteEndElement();
		}

		void WriteTravelTime(TravelTime tt) {
			_xmlWriter.WriteStartElement("travel-time");
			_xmlWriter.WriteAttributeString("start", tt.TimeStart.ToString());
			_xmlWriter.WriteAttributeString("end", tt.TimeEnd.ToString());
			
			foreach (var point in tt.Points) {
				WritePoint(point);
			}

			_xmlWriter.WriteEndElement();
		}

		void WritePoint(GPXPoint point) {
			_xmlWriter.WriteStartElement("pt");

			_xmlWriter.WriteAttributeString("lat", point.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture));
			_xmlWriter.WriteAttributeString("lon", point.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture));
			_xmlWriter.WriteAttributeString("time", point.Time.ToString());

			_xmlWriter.WriteEndElement();		
		}
		
		public void Load(string filename) {
			using(FileStream fs = new FileStream(filename, FileMode.Open)) {
				Load(fs);
			}
		}

		public void Load(Stream stream) {
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
							if (_xmlReader.Name != "travel-time-db")
								throw new XmlException("Invalid xml root element. Expected <travel-time-db>.");

							ReadRootTag();
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
		/// Reads the root element of the xml
		/// </summary>
		private void ReadRootTag() {
			_xmlReader.Read();

			while (_xmlReader.NodeType != XmlNodeType.EndElement) {
				switch (_xmlReader.Name) {
					case "segment":
						ReadSegment();
						break;
					default:
						_xmlReader.Skip();
						break;
				}
			}
		}

		/// <summary>
		/// Reads an OSMTag from the XmlTextReader and reurn it.
		/// </summary>
		/// <returns>The OSMTag read form the XmlTextReader</returns>
		private void ReadSegment() {
			SegmentInfo segment = new SegmentInfo();

			string attFrom = _xmlReader.GetAttribute("from");
			if (attFrom == null)
				throw new XmlException("Attribute 'from' is missing.");
			segment.NodeFromID = int.Parse(attFrom);

			string attTo = _xmlReader.GetAttribute("to");
			if (attTo == null)
				throw new XmlException("Attribute 'to' is missing.");
			segment.NodeToID = int.Parse(attTo);

			string attWay = _xmlReader.GetAttribute("way");
			if (attFrom == null)
				throw new XmlException("Attribute 'way' is missing.");
			segment.WayID = int.Parse(attWay);

			_storage.Add(segment, new List<TravelTime>());

			if (false == _xmlReader.IsEmptyElement) {
				_xmlReader.Read();

				while (_xmlReader.NodeType != XmlNodeType.EndElement) {
					switch (_xmlReader.NodeType) {
						case XmlNodeType.Element:
							switch (_xmlReader.Name) {
								case "travel-time":
									_storage[segment].Add(ReadTravelTime(segment));
									continue;
								default:
									_xmlReader.Skip();
									continue;
							}
						default:
							_xmlReader.Skip();
							break;
					}
				}
			}

			_xmlReader.Skip();
		}

		private TravelTime ReadTravelTime(SegmentInfo segment) {
			string attStart = _xmlReader.GetAttribute("start");
			if (attStart == null)
				throw new XmlException("Attribute 'start' is missing.");
			DateTime start = DateTime.Parse(attStart);

			string attEnd = _xmlReader.GetAttribute("end");
			if (attStart == null)
				throw new XmlException("Attribute 'end' is missing.");
			DateTime end = DateTime.Parse(attEnd);

			List<GPXPoint> points = new List<GPXPoint>();

			if (false == _xmlReader.IsEmptyElement) {
				_xmlReader.Read();

				while (_xmlReader.NodeType != XmlNodeType.EndElement) {
					switch (_xmlReader.NodeType) {
						case XmlNodeType.Element:
							switch (_xmlReader.Name) {
								case "pt":
									points.Add(ReadPoint());
									continue;
								default:
									_xmlReader.Skip();
									continue;
							}
						default:
							_xmlReader.Skip();
							break;
					}
				}
			}

			_xmlReader.Skip();

			return new TravelTime(segment, start, end, points);
		}

		private GPXPoint ReadPoint() {
			string lat = _xmlReader.GetAttribute("lat");
			if (string.IsNullOrEmpty(lat)) {
				throw new XmlException("Attribute 'lat' is missing.");
			}
			double pointLat = double.Parse(lat, System.Globalization.CultureInfo.InvariantCulture);


			string lon = _xmlReader.GetAttribute("lon");
			if (string.IsNullOrEmpty(lon)) {
				throw new XmlException("Attribute 'lon' is missing.");
			}
			double pointLon = double.Parse(lon, System.Globalization.CultureInfo.InvariantCulture);

			string timeAtt = _xmlReader.GetAttribute("time");
			if (string.IsNullOrEmpty(timeAtt)) {
				throw new XmlException("Attribute 'lon' is missing.");
			}
			DateTime time = DateTime.Parse(timeAtt);

			_xmlReader.Skip();

			return new GPXPoint(pointLat, pointLon, time);
		}
	}
}
