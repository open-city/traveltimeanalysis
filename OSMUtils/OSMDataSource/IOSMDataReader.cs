using System;
using System.Collections.Generic;
using System.Text;

using LK.OSMUtils.OSMDatabase;

namespace LK.OSMUtils.OSMDataSource {
	public delegate void OSMNodeReadHandler(OSMNode node);
	public delegate void OSMWayReadHandler(OSMWay way);
	public delegate void OSMRelationReadHandler(OSMRelation relation);

	public interface IOSMDataReader {
		/// <summary>
		/// Occurs when an OSMNode is read from the datasource
		/// </summary>
		event OSMNodeReadHandler NodeRead;

		/// <summary>
		/// Occurs when an OSMWay is read from the datasource
		/// </summary>
		event OSMWayReadHandler WayRead;

		/// <summary>
		/// Occurs when an OSMRelation is read from the datasource
		/// </summary>
		event OSMRelationReadHandler RelationRead;
	}
}
