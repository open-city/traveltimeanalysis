using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.MatchGPX2OSM {
	/// <summary>
	/// Represents candidates graph
	/// </summary>
	public class CandidatesGraph {
		private List<CandidateGraphLayer> _layers;
		/// <summary>
		/// Gets the list of layers of the graph, where every layer coresponds with one GPX point
		/// </summary>
		public IList<CandidateGraphLayer> Layers {
			get {
				return _layers;
			}
		}

		/// <summary>
		/// Creates a new instance of candidates graph
		/// </summary>
		public CandidatesGraph() {
			_layers = new List<CandidateGraphLayer>();
		}

		/// <summary>
		/// Creates connections among candidate points in subsequent layers
		/// </summary>
		public void ConnectLayers() {
			for (int l = 0; l < _layers.Count - 1; l++) {
				for (int i = 0; i < _layers[l].Candidates.Count; i++) {
					for (int j = 0; j < _layers[l + 1].Candidates.Count; j++) {
						ConnectPoints(_layers[l].Candidates[i], _layers[l + 1].Candidates[j]);
					}
				}
			}
		}

		/// <summary>
		/// Creates connection between two points
		/// </summary>
		/// <param name="from">The point, where the connections starts</param>
		/// <param name="to">The point, where the connection ends</param>
		void ConnectPoints(CandidatePoint from, CandidatePoint to) {
			CandidatesConection c = new CandidatesConection() { From = from, To = to };
			from.OutgoingConnections.Add(c);
			to.IncomingConnections.Add(c);


		}

	}
}
