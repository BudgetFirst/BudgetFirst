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

namespace BudgetFirst.Common.Infrastructure.Projections.Models
{
    using System;

    /// <summary>
    /// (Global) read store for read models.
    /// </summary>
    public interface IReadStore
    {
        /// <summary>
        /// Store an object for a specific id. Separate object types with same ID are stored and retrieved separately.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="id">Id of the object</param>
        /// <param name="obj">Object to store (overwrites existing)</param>
        void Store<T>(Guid id, T obj);

        /// <summary>
        /// Try to retrieve an object for a specific id.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="id">Object id</param>
        /// <param name="result">Is set when found</param>
        /// <returns><c>true</c> if the object could be found (and <see cref="result"/> is set).</returns>
        bool TryRetrieve<T>(Guid id, out T result);

        /// <summary>
        /// Retrieve an object for a specific id.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="id">Object id</param>
        /// <returns>Object, if the object could be found - default value otherwise.</returns>
        T Retrieve<T>(Guid id);

        /// <summary>
        /// Store an object. Separate object types are stored and retrieved separately.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="obj">Object to store (overwrites existing)</param>
        void StoreSingleton<T>(T obj);

        /// <summary>
        /// Try to retrieve an object
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="result">Is set when found</param>
        /// <returns><c>true</c> if the object could be found (and <see cref="result"/> is set).</returns>
        bool TryRetrieveSingleton<T>(out T result);

        /// <summary>
        /// Retrieve an object (or default if not found)
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <returns>Object, if the object could be found - default value otherwise.</returns>
        T RetrieveSingleton<T>();
    }
}