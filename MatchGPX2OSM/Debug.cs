using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LK.OSMUtils.OSMDatabase;

namespace LK.MatchGPX2OSM {
	static class Debug {
		public static void SaveCandidateIncomingConnections(CandidatePoint candidate, string filename) {
			int counter = -1;
			OSMDB result = new OSMDB();

			OSMNode osmCandidate = new OSMNode(counter--, candidate.Latitude, candidate.Longitude);
			osmCandidate.Tags.Add(new OSMTag("observation", candidate.ObservationProbability.ToString()));
			osmCandidate.Tags.Add(new OSMTag("time", candidate.Layer.TrackPoint.Time.ToString()));
			result.Nodes.Add(osmCandidate);
			foreach (var connection in candidate.IncomingConnections) {
				OSMNode from = new OSMNode(counter--, connection.From.Latitude, connection.From.Longitude);
				from.Tags.Add(new OSMTag("observation", connection.From.ObservationProbability.ToString()));
				from.Tags.Add(new OSMTag("time", connection.From.Layer.TrackPoint.Time.ToString()));
				result.Nodes.Add(from);

				OSMWay osmConnection = new OSMWay(counter--, new int[] { from.ID, osmCandidate.ID });
				osmConnection.Tags.Add(new OSMTag("transmission", connection.TransmissionProbability.ToString()));

				result.Ways.Add(osmConnection);
			}

			result.Save(filename);
		}
	}
}
