﻿//  Travel Time Analysis project
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

using LK.MatchGPX2OSM;
using LK.GPXUtils;
using LK.GeoUtils.Geometry;

namespace MatchGPX2OSM.Tests {
	public class CandidatesGraphTests {
		[Fact()]
		public void ConstructorInitializesProperties() {
			CandidatesGraph target = new CandidatesGraph();

			Assert.NotNull(target.Layers);
		}

		[Fact()]
		public void ConnectLayrsCreatesConnectionsAmongPoints() {
			CandidatesGraph target = new CandidatesGraph();

			CandidatePoint pt11 = new CandidatePoint() { MapPoint = new PointGeo() { Latitude = 1, Longitude = 1 } };
			CandidatePoint pt21 = new CandidatePoint() { MapPoint = new PointGeo() { Latitude = 2.1, Longitude = 2.1} };
			CandidatePoint pt22 = new CandidatePoint() { MapPoint = new PointGeo() { Latitude = 2.2, Longitude = 2.2} };

			CandidateGraphLayer layer1 = new CandidateGraphLayer();
			layer1.Candidates.Add(pt11);
			target.Layers.Add(layer1);

			CandidateGraphLayer layer2 = new CandidateGraphLayer();
			layer2.Candidates.AddRange(new CandidatePoint[] { pt21, pt22 });
			target.Layers.Add(layer2);

			target.ConnectLayers();

			Assert.Equal(0, target.Layers[0].Candidates[0].IncomingConnections.Count);
			Assert.Equal(1, target.Layers[0].Candidates[0].OutgoingConnections.Where(c => c.From == pt11 && c.To == pt21).Count());
			Assert.Equal(1, target.Layers[0].Candidates[0].OutgoingConnections.Where(c => c.From == pt11 && c.To == pt22).Count());

			Assert.Equal(0, target.Layers[1].Candidates[0].OutgoingConnections.Count);
			Assert.Equal(0, target.Layers[1].Candidates[1].OutgoingConnections.Count);

			Assert.Equal(1, target.Layers[1].Candidates[0].IncomingConnections.Where(c => c.From == pt11 && c.To == pt21).Count());
			Assert.Equal(1, target.Layers[1].Candidates[1].IncomingConnections.Where(c => c.From == pt11 && c.To == pt22).Count());
		}

		[Fact()]
		public void CreateLayerAddsLayerIntoGraph() {
			CandidatesGraph target = new CandidatesGraph();

			GPXPoint originalPt = new GPXPoint(1, 1.1);
			CandidatePoint pt1 = new CandidatePoint() { MapPoint = new PointGeo() { Latitude = 1, Longitude = 1 } };
			CandidatePoint pt2 = new CandidatePoint() { MapPoint = new PointGeo() { Latitude = 2.1, Longitude = 2.1 } };

			target.CreateLayer(originalPt, new CandidatePoint[] { pt1, pt2 });

			Assert.Equal(1, target.Layers.Count);
			Assert.Equal(2, target.Layers[0].Candidates.Count);
		}

		[Fact()]
		public void CreateLayerAddsOriginalPointAndCandidatePointsIntoLayer() {
			CandidatesGraph target = new CandidatesGraph();

			GPXPoint originalPt = new GPXPoint(1, 1.1);
			CandidatePoint pt1 = new CandidatePoint() { MapPoint = new PointGeo() { Latitude = 1, Longitude = 1 } };
			CandidatePoint pt2 = new CandidatePoint() { MapPoint = new PointGeo() { Latitude = 2.1, Longitude = 2.1 } };

			target.CreateLayer(originalPt, new CandidatePoint[] { pt1, pt2 });

			Assert.Equal(originalPt, target.Layers[0].TrackPoint);

			Assert.Equal(2, target.Layers[0].Candidates.Count);
			Assert.Equal(pt1, target.Layers[0].Candidates[0]);
			Assert.Equal(pt2, target.Layers[0].Candidates[1]);
		}
	}
}
