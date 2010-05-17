using System;
using System.Collections.Generic;
using System.Text;

using LK.OSMUtils.OSMDatabase;

namespace LK.OSMUtils.OSMDataSource {
	public interface IOSMDataWriter {
		/// <summary>
		/// Writes the specific OSMNode to the data source
		/// </summary>
		/// <param name="node">The OSMNode to be written.</param>
		void WriteNode(OSMNode node);

		/// <summary>
		/// Writes the specific OSMWay to the data source
		/// </summary>
		/// <param name="way">The OSMWay to be written.</param>
		void WriteWay(OSMWay way);

		/// <summary>
		/// Writes the specific OSMRelation to the data source
		/// </summary>
		/// <param name="relation">The OSMRelation to be written.</param>
		void WriteRelation(OSMRelation relation);
	}
}
