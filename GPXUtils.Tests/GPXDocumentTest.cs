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
