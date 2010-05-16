using System;

/// <summary>
/// Represents the type of the relation member
/// </summary>
public enum OSMRelationMemberType {
	// Relation member is a node
	node,

	// Relation member is a way
	way,

	// Relation member is a relation
	relation
}