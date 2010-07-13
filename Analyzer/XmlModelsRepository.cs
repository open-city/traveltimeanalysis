using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace LK.Analyzer {
	public class XmlModelsRepository : MemoryModelsRepository, IDisposable {
		Stream _xmlStream;
		XmlWriter _xmlWriter;

		public XmlModelsRepository(Stream stream) {
			_xmlStream = stream;
		}

		public XmlModelsRepository(string filename) {
			_xmlStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		}

		protected void Load() {
		}

		protected void Save() {
			XmlWriterSettings writerSetting = new XmlWriterSettings();
			writerSetting.Indent = true;

			_xmlStream.Seek(0, 0);
			_xmlStream.SetLength(0);
			_xmlWriter = XmlTextWriter.Create(new StreamWriter(_xmlStream, new UTF8Encoding(false)), writerSetting);

			_xmlWriter.WriteStartElement("travel-time-db");

			foreach (var segment in _storage.Keys) {
				WriteModel(segment);
			}

			_xmlWriter.WriteEndElement();
			_xmlWriter.Close();
		}

		void WriteModel(SegmentInfo segment) {
			_xmlWriter.WriteStartElement("model");
			_xmlWriter.WriteAttributeString("node-from", segment.NodeFromID.ToString());
			_xmlWriter.WriteAttributeString("node-to", segment.NodeToID.ToString());
			_xmlWriter.WriteAttributeString("way", segment.WayID.ToString());

			Model model = _storage[segment];
			_xmlWriter.WriteAttributeString("freeflow", model.FreeFlowTravelTime.ToString(System.Globalization.CultureInfo.InvariantCulture));
			_xmlWriter.WriteAttributeString("signals-delay", model.TrafficSignalsDelay.Length.ToString(System.Globalization.CultureInfo.InvariantCulture));
			_xmlWriter.WriteAttributeString("signals-probability", model.TrafficSignalsDelay.Probability.ToString(System.Globalization.CultureInfo.InvariantCulture));

			foreach (var delay in model.TrafficDelay) {
				WriteDelay(delay);
			}

			_xmlWriter.WriteEndElement();
		}

		void WriteDelay(TrafficDelayInfo delay) {
			_xmlWriter.WriteStartElement("traffic-delay");
			_xmlWriter.WriteAttributeString("from", delay.From.ToString());
			_xmlWriter.WriteAttributeString("to", delay.To.ToString());
			_xmlWriter.WriteAttributeString("day", delay.Day.ToString());

			_xmlWriter.WriteAttributeString("delay", delay.Delay.ToString(System.Globalization.CultureInfo.InvariantCulture));

			_xmlWriter.WriteEndElement();
		}

		public override void Commit() {
			Save();
		}


		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				if (_xmlStream != null) {
					_xmlStream.Close();
					_xmlStream.Dispose();
					_xmlStream = null;
				}
			}
		}
	}
}
