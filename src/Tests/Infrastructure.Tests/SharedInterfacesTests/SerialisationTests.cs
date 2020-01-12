// BudgetFirst 
// ©2020 Sabrina Mühlgrabner
// This source code is dual-licensed under:
//   * Mozilla Public License 2.0 (MPL 2.0) 
//   * GNU General Public License v3.0 (GPLv3)
// ==================== Mozilla Public License 2.0 ===================
// This Source Code Form is subject to the terms of the Mozilla Public 
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// ================= GNU General Public License v3.0 =================
// This file is part of BudgetFirst.
// BudgetFirst is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// BudgetFirst is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with Budget First.  If not, see<http://www.gnu.org/licenses/>.
// ===================================================================

namespace BudgetFirst.Infrastructure.Tests.SharedInterfacesTests
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;

    using BudgetFirst.Common.Infrastructure.Serialisation;

    using NUnit.Framework;

    /// <summary>
    /// Contains tests for serialisation and de-serialisation
    /// </summary>
    [TestFixture]
    public class SerialisationTests
    {
        /// <summary>
        /// Verifies that an object, after a full serialisation roundtrip, contains all values that were annotated with <see cref="DataMember"/>.
        /// This includes private members.
        /// </summary>
        [Test]
        public void SerialisationRoundtripIncludesAllDataAnnotatedMembers()
        {
            const double ExpectedDoubleValue = 12.34d;
            const string IncludedStringProperty = "Spanish inquisition";
            const long IncludedLongProperty = 1234567890;

            var serialisable = new SerialisableTestClass(ExpectedDoubleValue)
                                   {
                                       ExcludedStringPoperty = "Excluded string", 
                                       IncludedLongProperty = IncludedLongProperty, 
                                       IncludedStringProperty = IncludedStringProperty, 
                                   };

            var result = this.SerialisationRoundtrip(serialisable);

            Assert.AreEqual(ExpectedDoubleValue, result.GetIncludedPrivateDoubleValue());
            Assert.AreEqual(IncludedLongProperty, result.IncludedLongProperty);
            Assert.AreEqual(IncludedStringProperty, result.IncludedStringProperty);
        }

        /// <summary>
        /// Verifies that an object, after a full serialisation roundtrip, contains ONLY values that were annotated with <see cref="DataMember"/>.
        /// </summary>
        [Test]
        public void SerialisationRoundtripIncludesOnlyDataAnnotatedMembers()
        {
            const double ExpectedDoubleValue = 12.34d;
            const string IncludedStringProperty = "Spanish inquisition";
            const long IncludedLongProperty = 1234567890;

            var serialisable = new SerialisableTestClass(ExpectedDoubleValue)
                                   {
                                       ExcludedStringPoperty = "Excluded string", 
                                       IncludedLongProperty = IncludedLongProperty, 
                                       IncludedStringProperty = IncludedStringProperty, 
                                   };

            var result = this.SerialisationRoundtrip(serialisable);

            Assert.AreEqual(null, result.ExcludedStringPoperty);
            Assert.AreEqual(0, result.GetExcludedPrivateDoubleValue());
        }

        /// <summary>
        /// Perform a serialisation roundtrip
        /// </summary>
        /// <param name="source">Source object to serialise and de-serialise</param>
        /// <returns>Clone of source object through serialisation roundtrip</returns>
        private SerialisableTestClass SerialisationRoundtrip(SerialisableTestClass source)
        {
            return Serialiser.CloneSerialisable(source);
        }

        /// <summary>
        /// Test class that contains a few members and properties to demonstrate which values are serialised and which are not.
        /// The intent here is to show that only annotated members are serialised and de-serialised - which is the behaviour we expect.
        /// </summary>
        [DataContract(Name = "SerialisableTestClass", Namespace = "http://budgetfirst.github.io/Schemas/SerialisationTests")]
        private class SerialisableTestClass
        {
            /// <summary>
            /// Private member which should be excluded during serialisation
            /// </summary>
            /// <remarks>Do not attribute with <see cref="DataMemberAttribute"/></remarks>
            private double excludedDoublePrivateMember;

            /// <summary>
            /// Private member which is still included in serialisation
            /// </summary>
            [DataMember(Name = "IncludedDoublePrivateMember")]
            private double includedDoublePrivateMember;

            /// <summary>
            /// Initialises a new instance of the <see cref="SerialisableTestClass"/> class.
            /// </summary>
            /// <param name="doubleValue">Double value to set</param>
            public SerialisableTestClass(double doubleValue)
            {
                this.includedDoublePrivateMember = doubleValue;
                this.excludedDoublePrivateMember = doubleValue * 2;
            }

            /// <summary>
            /// Gets or sets a string property, which is excluded during de-serialisation.
            /// </summary>
            /// <remarks>Do not attribute with <see cref="DataMemberAttribute"/></remarks>
            public string ExcludedStringPoperty { get; set; }

            /// <summary>
            /// Gets or sets a long property, which is included in serialisation
            /// </summary>
            [DataMember(Name = "IncludedLongProperty")]
            public long IncludedLongProperty { get; set; }

            /// <summary>
            /// Gets or sets a string property, which is included in serialisation
            /// </summary>
            [DataMember(Name = "IncludedStringProperty")]
            public string IncludedStringProperty { get; set; }

            /// <summary>
            /// Get the double value from the private member
            /// </summary>
            /// <returns>Value from private member</returns>
            public double GetExcludedPrivateDoubleValue()
            {
                return this.excludedDoublePrivateMember;
            }

            /// <summary>
            /// Get the double value from the private member
            /// </summary>
            /// <returns>Value from private member</returns>
            public double GetIncludedPrivateDoubleValue()
            {
                return this.includedDoublePrivateMember;
            }
        }
    }
}