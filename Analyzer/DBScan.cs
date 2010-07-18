﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Analyzer {
			class ItemState {
			public bool Visited;
			public bool InCluster;
		}


	public class DBScan<T> {
		public delegate List<T> FindNeighbours(T target, IList<T> items);

		private double _eps;
		private int _minClusterMembersCount;
		private Dictionary<T, ItemState> _itemsStates;
		private FindNeighbours _findNeighboursCallback;

		public DBScan(FindNeighbours callback) {
			_findNeighboursCallback = callback;
		}
		
		public List<List<T>> ClusterAnalysis(List<T> items, int minCount) {
			_minClusterMembersCount = minCount;

			//ititialize
			List<List<T>> clusters = new List<List<T>>();
			_itemsStates = new Dictionary<T, ItemState>();
			foreach (var item in items) {
				_itemsStates.Add(item, new ItemState() { Visited = false, InCluster = false });
			}

			foreach (var item in items) {
				if (_itemsStates[item].Visited == false) {
					_itemsStates[item].Visited = true;

					var neighbours = _findNeighboursCallback(item, items);
					if (neighbours.Count >= _minClusterMembersCount -1) {
						List<T> cluster = new List<T>();
						clusters.Add(cluster);

						cluster.Add(item);
						ExpandCluster(cluster, neighbours, items);
					}
				}
			}

			return clusters;
		}

		void ExpandCluster(List<T> cluster, List<T> neighbours, List<T> items) {
			int index = 0;
			while (index < neighbours.Count) {
				if (_itemsStates[neighbours[index]].Visited == false) {
					_itemsStates[neighbours[index]].Visited = true;

					var neighbours_ = _findNeighboursCallback(neighbours[index], items);
					if (neighbours_.Count >= _minClusterMembersCount -1) {
						foreach (var neighbour in neighbours_) {
							if (neighbours.Contains(neighbour) == false) {
								neighbours.Add(neighbour);
							}
						}
					}
				}

				if (_itemsStates[neighbours[index]].InCluster == false) {
					cluster.Add(neighbours[index]);
					_itemsStates[neighbours[index]].InCluster = true;
				}

				index++;
			}
		}
	}
}
