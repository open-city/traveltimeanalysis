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

		public IEnumerable<SegmentInfo> TravelTimesSegments {
			get {
				return _storage.Keys;
			}
		}

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
			_xmlWriter.WriteAttributeString("start", tt.TimeStart.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f"));
			_xmlWriter.WriteAttributeString("end", tt.TimeEnd.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f"));

			foreach (var stop in tt.Stops) {
				WriteStop(stop);
			}

			_xmlWriter.WriteEndElement();
		}

		void WriteStop(Stop stop) {
			_xmlWriter.WriteStartElement("stop");

			_xmlWriter.WriteAttributeString("start", stop.Start.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f"));
			_xmlWriter.WriteAttributeString("end", stop.End.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f"));

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

							if(_xmlReader.IsEmptyElement == false)
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

			List<Stop> stops = new List<Stop>();

			if (false == _xmlReader.IsEmptyElement) {
				_xmlReader.Read();

				while (_xmlReader.NodeType != XmlNodeType.EndElement) {
					switch (_xmlReader.NodeType) {
						case XmlNodeType.Element:
							switch (_xmlReader.Name) {
								case "stop":
									stops.Add(ReadStop());
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

			return new TravelTime(segment, start, end, stops);
		}

		private Stop ReadStop() {
			string startAtt = _xmlReader.GetAttribute("start");
			if (string.IsNullOrEmpty(startAtt)) {
				throw new XmlException("Attribute 'start' is missing.");
			}
			DateTime start = DateTime.Parse(startAtt);

			string endAtt = _xmlReader.GetAttribute("end");
			if (string.IsNullOrEmpty(endAtt)) {
				throw new XmlException("Attribute 'end' is missing.");
			}
			DateTime end = DateTime.Parse(endAtt);

			_xmlReader.Skip();

			return new Stop() { Start = start, End = end };
		}
	}
}
