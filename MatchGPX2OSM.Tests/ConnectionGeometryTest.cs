using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using LK.MatchGPX2OSM;

namespace MatchGPX2OSM.Tests {
	public class ConnectionGeometryTest {
		[Fact()]
		public void ConnectionGeometryConstructorInitializesInternalFileds() {
			ConnectionGeometry target = new ConnectionGeometry();

			Assert.NotNull(target.Connections);
		}
	}
}
