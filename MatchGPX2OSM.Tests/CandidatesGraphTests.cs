using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.MatchGPX2OSM;

namespace MatchGPX2OSM.Tests {
	public class CandidatesGraphTests {
		[Fact()]
		public void CandidatesGraphConstructorInitializesProperties() {
			CandidatesGraph target = new CandidatesGraph();

			Assert.NotNull(target.Layers);
		}

		[Fact()]
		public void CandidatesGraphConnectLayrsCreatesConnectionsAmongPoints() {
			CandidatesGraph target = new CandidatesGraph();

			CandidatePoint pt11 = new CandidatePoint() { Latitude = 1, Longitude = 1 };
			CandidatePoint pt21 = new CandidatePoint() { Latitude = 2.1, Longitude = 2.1 };
			CandidatePoint pt22 = new CandidatePoint() { Latitude = 2.2, Longitude = 2.2 };

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
	}
}
