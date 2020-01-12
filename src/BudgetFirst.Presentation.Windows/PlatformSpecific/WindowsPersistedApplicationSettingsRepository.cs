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

namespace BudgetFirst.Presentation.Windows.PlatformSpecific
{
    using System.IO;

    using BudgetFirst.Common.Infrastructure.Persistency;
    using BudgetFirst.Common.Infrastructure.Serialisation;
    
    /// <summary>
    /// Provides access to the application settings on windows platform
    /// </summary>
    public class WindowsPersistedApplicationSettingsRepository : IPersistedApplicationStateRepository
    {
        /// <summary>
        /// Get the current application state from disk
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>Application state, if it could be loaded</returns>
        public PersistableApplicationState Get(string path)
        {
            using (var filestream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return Serialiser.DeSerialise<PersistableApplicationState>(filestream);
            }
        }

        /// <summary>
        /// Save the application state to disk (or other location)
        /// </summary>
        /// <param name="state">Current application state</param>
        /// <param name="location">Platform-specific identifier of where to store the state to (path, key, etc.)</param>
        public void Save(PersistableApplicationState state, string location)
        {
            using (var filestream = new FileStream(location, FileMode.Create, FileAccess.Write))
            {
                using (var memorystream = new MemoryStream())
                {
                    Serialiser.Serialise(state, memorystream);
                    memorystream.Position = 0;
                    memorystream.CopyTo(filestream);
                }
            }
        }
    }
}