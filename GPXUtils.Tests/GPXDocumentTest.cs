using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.GPXUtils;
using LK.GPXUtils.GPXDataSource;
using System.IO;

namespace GPXUtils.Tests {
	public class GPXDocumentTest {
		[Fact()]
		public void GPXDocumentConstructorInitializesCollections() {
			GPXDocument target = new GPXDocument();

			Assert.NotNull(target.Waypoints);
			Assert.NotNull(target.Routes);
			Assert.NotNull(target.Tracks);
		}

		[Fact()]
		public void GPXDocumentLoadReadsDataFromStream() {
			GPXDocument target = new GPXDocument();
			target.Load(new MemoryStream(TestData.gpx_real));

			Assert.Equal(3, target.Waypoints.Count);

			Assert.Equal(2, target.Routes.Count);
			Assert.Equal(3, target.Routes[0].NodesCount);
			Assert.Equal(1, target.Routes[1].NodesCount);

			Assert.Equal(2, target.Tracks.Count);
			
			Assert.Equal(1, target.Tracks[0].Segments.Count);
			Assert.Equal(2, target.Tracks[0].Segments[0].NodesCount);

			Assert.Equal(2, target.Tracks[1].Segments.Count);
			Assert.Equal(998, target.Tracks[1].Segments[0].NodesCount);
			Assert.Equal(10, target.Tracks[1].Segments[1].NodesCount);
		}

		[Fact()]
		public void GPXDocumentSaveWritesDataToStream() {
			GPXDocument target = new GPXDocument();
			target.Load(new MemoryStream(TestData.gpx_real));

			MemoryStream writtenDocument = new MemoryStream();
			target.Save(writtenDocument);

			writtenDocument.Seek(0, 0);
			GPXDocument loadedDocument = new GPXDocument();
			loadedDocument.Load(writtenDocument);

			Assert.Equal(3, loadedDocument.Waypoints.Count);

			Assert.Equal(2, loadedDocument.Routes.Count);
			Assert.Equal(3, loadedDocument.Routes[0].NodesCount);
			Assert.Equal(1, loadedDocument.Routes[1].NodesCount);

			Assert.Equal(2, loadedDocument.Tracks.Count);

			Assert.Equal(1, loadedDocument.Tracks[0].Segments.Count);
			Assert.Equal(2, loadedDocument.Tracks[0].Segments[0].NodesCount);

			Assert.Equal(2, loadedDocument.Tracks[1].Segments.Count);
			Assert.Equal(998, loadedDocument.Tracks[1].Segments[0].NodesCount);
			Assert.Equal(10, loadedDocument.Tracks[1].Segments[1].NodesCount);
		}
	}
}
