using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Analyzer {
	public class MemoryModelsRepository : IModelsRepository {
		protected Dictionary<SegmentInfo, Model> _storage;

		public MemoryModelsRepository() {
			_storage = new Dictionary<SegmentInfo, Model>();
		}

		public void AddModel(Model model) {
			if (_storage.ContainsKey(model.Segment) == false)
				_storage.Add(model.Segment, model);
			else
				_storage[model.Segment] = model;
		}

		public Model GetModel(SegmentInfo segment) {
			if (_storage.ContainsKey(segment))
				return _storage[segment];
			else
				return null;
		}

		public IEnumerable<Model> GetModels() {
			return _storage.Values;
		}

		public virtual void Commit() {
			;
		}

	}
}
