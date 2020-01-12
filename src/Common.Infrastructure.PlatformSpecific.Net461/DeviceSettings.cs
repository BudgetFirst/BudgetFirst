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

namespace BudgetFirst.Common.Infrastructure.PlatformSpecific.Net461
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    using BudgetFirst.Common.Infrastructure.Persistency;
    using BudgetFirst.Common.Infrastructure.PlatformSpecific.Net461.Properties;
    using BudgetFirst.Common.Infrastructure.Serialisation;

    /// <summary>
    /// Device settings for .net 4.6.1 platforms (i.e. Windows or Linux)
    /// </summary>
    public class DeviceSettings : IDeviceSettings
    {
        /// <summary>
        /// Settings repository
        /// </summary>
        private SettingsRepository repository = new SettingsRepository();

        /// <summary>
        /// Get the device Id
        /// </summary>
        /// <returns>Device Id</returns>
        public Guid GetDeviceId()
        {
            // TODO: extract the behaviour, it should be the same for every platform:
            // Read persisted device Id - or generate one (and save it) if it doesn't exist yet
            var settings = this.repository.GetSettings();
            if (settings.DeviceId == Guid.Empty)
            {
                settings.DeviceId = Guid.NewGuid();
                settings.Save();
            }

            return settings.DeviceId;
        }

        /// <summary>
        /// Set the automatically loaded budget identifier
        /// </summary>
        /// <param name="location">Identifier for the automatically loaded budget</param>
        public void SetAutoloadBudgetIdentifier(string location)
        {
            var settings = this.repository.GetSettings();
            settings.AutoloadBudgetIdentifier = location;
            settings.Save();
        }

        /// <summary>
        /// Get the automatically loaded budget identifier
        /// </summary>
        /// <returns>Identifier for the automatically loaded budget. Might be <c>null</c> or empty if not set.</returns>
        public string GetAutoloadBudgetIdentifier()
        {
            return this.repository.GetSettings().AutoloadBudgetIdentifier;
        }

        /// <summary>
        /// Get the recent budgets
        /// </summary>
        /// <returns>Recent budgets</returns>
        public List<RecentBudget> GetRecentBudgets()
        {
            var recentBudgetsSerialised = this.repository.GetSettings().RecentBudgetsSerialised;

            if (string.IsNullOrWhiteSpace(recentBudgetsSerialised))
            {
                return null;
            }

            // Memory stream is disposed when stream writer is disposed
            var memorystream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memorystream))
            {
                streamWriter.Write(recentBudgetsSerialised);
                streamWriter.Flush();
                memorystream.Position = 0;

                var container = Serialiser.DeSerialise<RecentBudgetsContainer>(memorystream);
                return container.RecentBudgets;
            }
        }

        /// <summary>
        /// Add a budget to the list of recent budgets
        /// </summary>
        /// <param name="recentBudget">Recent budget to add</param>
        public void AddRecentBudget(RecentBudget recentBudget)
        {
            var existing = this.GetRecentBudgets();
            if (existing == null)
            {
                existing = new List<RecentBudget>();
            }
            
            // add or update position
            var matching = existing.FirstOrDefault(x => x.Identifier == recentBudget.Identifier);
            if (matching == null)
            {
                existing.Add(recentBudget);
            }
            else
            {
                matching.DisplayName = recentBudget.DisplayName;
                existing.Remove(matching);
                existing.Add(matching);
            }

            // constrain size to something reasonable
            // TODO: should be the same behaviour for each platform?
            if (existing.Count > 20)
            {
                existing.RemoveRange(0, existing.Count - 20);
            }

            var container = new RecentBudgetsContainer() { RecentBudgets = existing };

            string serialised;
            using (var memoryStream = new MemoryStream())
            {
                Serialiser.Serialise(container, memoryStream);
                memoryStream.Position = 0; // rewind
                var bytes = memoryStream.ToArray();
                serialised = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }

            var settings = this.repository.GetSettings();
            settings.RecentBudgetsSerialised = serialised;
            settings.Save();
        }

        /// <summary>
        /// Container for recent budgets
        /// </summary>
        [DataContract(Name = "RecentBudgetsContainer", Namespace = "http://budgetfirst.github.io/schemas/2016/08/06/RecentBudgetsContainer")]
        private class RecentBudgetsContainer
        {
            /// <summary>
            /// Gets or sets list of recent budgets
            /// </summary>
            [DataMember(Name = "RecentBudgets")]
            public List<RecentBudget> RecentBudgets { get; set; }
        }
    }
}