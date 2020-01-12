// BudgetFirst 
// ©2020 Sabrina Mühlgrabner
//
// This source code is dual-licensed under:
//   * Mozilla Public License 2.0 (MPL 2.0) 
//   * GNU General Public License v3.0 (GPLv3)
//
// ==================== Mozilla Public License 2.0 ===================
// This Source Code Form is subject to the terms of the Mozilla Public 
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
// ================= GNU General Public License v3.0 =================
// This file is part of BudgetFirst.
//
// BudgetFirst is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BudgetFirst is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Budget First.  If not, see<http://www.gnu.org/licenses/>.
// ===================================================================

namespace BudgetFirst.Infrastructure.Tests.SharedInterfacesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BudgetFirst.Common.Infrastructure.Domain.Events;
    using BudgetFirst.Common.Infrastructure.Messaging;

    using NUnit.Framework;

    /// <summary>
    /// Contains tests for vector clocks
    /// </summary>
    [TestFixture]
    public class VectorClockTests
    {
        /// <summary>
        /// New vector clocks start at 1
        /// </summary>
        [Test]
        public void StartAtOne()
        {
            var clock = new VectorClock();
            Assert.That(!clock.ContainsKey("key1"));

            clock = clock.Increment("key1");
            Assert.That(clock["key1"] == 1);
        }

        /// <summary>
        /// Incrementing vector clocks increments by 1
        /// </summary>
        [Test]
        public void IncrementClock()
        {
            var vector = new Dictionary<string, int>();
            vector["key1"] = 2;
            vector["key2"] = 3;
            var clock = new VectorClock(vector);

            clock = clock.Increment("key1");
            clock = clock.Increment("key2");

            Assert.That(clock["key1"] == 3 && clock["key2"] == 4);
        }

        /// <summary>
        /// Tests merging of vector clocks
        /// </summary>
        [Test]
        public void MergeClock()
        {
            var clock1 = new VectorClock();
            clock1 = clock1.Increment("key1");
            clock1 = clock1.Increment("key1");
            clock1 = clock1.Increment("key1");

            var clock2 = new VectorClock();
            clock2 = clock2.Increment("key2");
            clock2 = clock2.Increment("key2");

            var clock3 = new VectorClock();
            clock3 = clock3.Increment("key3");

            var mergedClock = new VectorClock();
            mergedClock = mergedClock.Merge(clock1);
            mergedClock = mergedClock.Increment("key1");
            mergedClock = mergedClock.Merge(clock2);
            mergedClock = mergedClock.Merge(clock3);

            Assert.That(mergedClock["key1"] == 4 && mergedClock["key2"] == 2 && mergedClock["key3"] == 1);

            // Merge should not affect the original VectorClock
            Assert.That(clock1["key1"] == 3);
        }

        /// <summary>
        /// Tests copying of vector clocks
        /// </summary>
        [Test]
        public void CopyClock()
        {
            var clock1 = new VectorClock();
            clock1 = clock1.Increment("key1");
            clock1 = clock1.Increment("key1");
            clock1 = clock1.Increment("key1");

            var clock2 = clock1.Copy();
            clock2 = clock2.Increment("key1");

            Assert.That(clock1 != clock2);
            Assert.That(clock1["key1"] == 3);
            Assert.That(clock2["key1"] == 4);
        }

        /// <summary>
        /// Tests comparing of vector clocks
        /// </summary>
        [Test]
        public void CompareVectors()
        {
            var clock1 = new VectorClock();
            clock1 = clock1.Increment("key1");

            var clock2 = clock1.Copy();
            clock2 = clock2.Increment("key2");

            var clock3 = new VectorClock();
            clock3 = clock3.Increment("key1");
            clock3 = clock3.Increment("key2");

            Assert.That(clock1.CompareVectors(clock2) == VectorClock.ComparisonResult.Smaller);
            Assert.That(clock2.CompareVectors(clock1) == VectorClock.ComparisonResult.Greater);
            Assert.That(clock3.CompareVectors(clock2) == VectorClock.ComparisonResult.Equal);
        }

        /// <summary>
        /// Simultaneous operations are detected
        /// </summary>
        [Test]
        public void DetectSimultaneousOperations()
        {
            var baseClock = new VectorClock();
            baseClock = baseClock.Increment("key1");
            baseClock = baseClock.Increment("key1");

            var clock1 = baseClock.Copy();
            var clock2 = baseClock.Copy();
            var clock3 = baseClock.Copy();
            clock1 = clock1.Increment("key1");
            clock2 = clock2.Increment("key2");
            clock3 = clock3.Increment("key3");

            Assert.That(clock1.CompareVectors(clock2) == VectorClock.ComparisonResult.Simultaneous);
            Assert.That(clock2.CompareVectors(clock1) == VectorClock.ComparisonResult.Simultaneous);

            Assert.That(clock1.CompareVectors(clock3) == VectorClock.ComparisonResult.Simultaneous);
            Assert.That(clock3.CompareVectors(clock1) == VectorClock.ComparisonResult.Simultaneous);

            Assert.That(clock3.CompareVectors(clock2) == VectorClock.ComparisonResult.Simultaneous);
            Assert.That(clock2.CompareVectors(clock3) == VectorClock.ComparisonResult.Simultaneous);

            var clock4 = clock1.Merge(clock2);
            clock4 = clock4.Merge(clock3);

            Assert.That(clock4.CompareVectors(clock1) == VectorClock.ComparisonResult.Greater);
            Assert.That(clock4.CompareVectors(clock2) == VectorClock.ComparisonResult.Greater);
            Assert.That(clock4.CompareVectors(clock3) == VectorClock.ComparisonResult.Greater);
            Assert.That(clock1.CompareVectors(clock4) == VectorClock.ComparisonResult.Smaller);
            Assert.That(clock2.CompareVectors(clock4) == VectorClock.ComparisonResult.Smaller);
            Assert.That(clock3.CompareVectors(clock4) == VectorClock.ComparisonResult.Smaller);

            // All previous clocks  descend from baseClock which has the vector ["key1" : 2]
            var otherClock = new VectorClock();
            otherClock = otherClock.Increment("key4");

            // otherClock should be simultaneous with all of them since it doesnt recognize ["key1" : 2]
            Assert.That(otherClock.CompareVectors(baseClock) == VectorClock.ComparisonResult.Simultaneous);
            Assert.That(otherClock.CompareVectors(clock1) == VectorClock.ComparisonResult.Simultaneous);
            Assert.That(otherClock.CompareVectors(clock2) == VectorClock.ComparisonResult.Simultaneous);
            Assert.That(otherClock.CompareVectors(clock3) == VectorClock.ComparisonResult.Simultaneous);
            Assert.That(otherClock.CompareVectors(clock4) == VectorClock.ComparisonResult.Simultaneous);
        }
    }
}
