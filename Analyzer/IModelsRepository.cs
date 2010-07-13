using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Analyzer {
	public interface IModelsRepository {
		void AddModel(Model model);
		Model GetModel(SegmentInfo segment);
		IEnumerable<Model> GetModels();

		void Commit();
	}
}
