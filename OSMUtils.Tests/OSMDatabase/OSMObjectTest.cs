//  Travel Time Analysis project
//  Copyright (C) 2010 Lukas Kabrt
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using LK.OSMUtils.OSMDatabase;
using Xunit;
using System;

namespace OSMUtils.Tests
{
    
    
    /// <summary>
    ///This is a test class for OSMObjectTest and is intended
    ///to contain all OSMObjectTest Unit Tests
    ///</summary>
	
	public class OSMObjectTest {
		private class OSMObjectWrapper : OSMObject {
			public OSMObjectWrapper(int id)
				: base(id) {
			}
		}


		[Fact()]
		public void OSMObjectConstructorInitializesProperties() {
			int id = 1374;

			OSMObject target = new OSMObjectWrapper(id);

			Assert.Equal(id, target.ID);
			Assert.NotNull(target.Tags);
		}

		[Fact()]
		public void OSMObjectToStringReturnsTheStringRepresentationOfTheID() {
			int id = 1374;

			OSMObjectWrapper target = new OSMObjectWrapper(id);

			Assert.Equal(id.ToString(), target.ToString());
		}

		[Fact()]
		public void OSMObjectGetHashCodeComputeHashFromTheID() {
			int id = 1374;

			OSMObjectWrapper target = new OSMObjectWrapper(id);

			Assert.Equal(id.GetHashCode(), target.GetHashCode());
		}

		[Fact()]
		public void OSMObjectEqualsComparesObjcetsByID() {
			OSMObjectWrapper target = new OSMObjectWrapper(1374);
			OSMObjectWrapper equalsTaregt = new OSMObjectWrapper(1374);

			Assert.True(target.Equals(equalsTaregt));


			OSMObjectWrapper otherObject = new OSMObjectWrapper(1);

			Assert.False(target.Equals(otherObject));
		}
	}
}
