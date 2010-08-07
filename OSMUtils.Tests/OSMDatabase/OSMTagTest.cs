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
    ///This is a test class for OSMTagTest and is intended
    ///to contain all OSMTagTest Unit Tests
    ///</summary>
	
	public class OSMTagTest {
		[Fact()]
		public void OSMTagConstructorSetsKeyAndValue() {
			string key = "test-key";
			string value = "test-value";

			OSMTag target = new OSMTag(key, value);

			Assert.Equal(key, target.Key);
			Assert.Equal(value, target.Value);
		}

		[Fact()]
		public void OSMTagConstructorThrowsExceptionIfKeyIsNull() {
			string key = null;
			string value = "test-value";

            Assert.Throws<ArgumentException>(delegate { new OSMTag(key, value); });
		}

		[Fact()]
		public void OSMTagConstructorThrowsExceptionIfKeyIsEmpty() {
			string key = "";
			string value = "test-value";

            Assert.Throws<ArgumentException>(delegate { new OSMTag(key, value); });
		}

		[Fact()]
		public void OSMTagConstructorThrowsExceptionIfValueIsNull() {
			string key = "test-key";
			string value = null;

            Assert.Throws<ArgumentNullException>(delegate { new OSMTag(key, value); });
		}

		[Fact()]
		public void OSMTagToStringReturnsCorrectStringRepresentationOfTheObject() {
			string key = "test-key";
			string value = "test-value";
			string stringRepresentation = "test-key = test-value";

			OSMTag target = new OSMTag(key, value);

			Assert.Equal(stringRepresentation, target.ToString());
		}

		[Fact()]
		public void OSMTagEqualsComparesKeyAndValue() {
			OSMTag target = new OSMTag("test-key", "test-value");
			OSMTag equalsTarget = new OSMTag("test-key", "test-value");

			Assert.True(target.Equals(equalsTarget));

			OSMTag differentKeyTarget = new OSMTag("different-key", "test-value");
			Assert.False(target.Equals(differentKeyTarget));

			OSMTag differentValueTarget = new OSMTag("test-key", "different-value");
			Assert.False(target.Equals(differentValueTarget));
		}

		[Fact()]
		public void OSMTagGetHashCodeComputeHashFromKeyAndValue() {
			string key = "test-key";
			string value = "test-value";
			int expectedHashCode = unchecked( key.GetHashCode()*83 + value.GetHashCode() );

			OSMTag target = new OSMTag(key, value);

			Assert.Equal(expectedHashCode, target.GetHashCode());
		}
	}
}
