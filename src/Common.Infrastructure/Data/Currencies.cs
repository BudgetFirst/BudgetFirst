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

namespace BudgetFirst.Common.Infrastructure.Data
{
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    /// <summary>
    /// Contains the (raw) list of currencies
    /// </summary>
    /// <remarks>TODO: move into own context</remarks>
    public class Currencies
    {
        /// <summary>
        /// Cache of the document
        /// </summary>
        private XDocument cache;

        /// <summary>
        /// Initialises a new instance of the <see cref="Currencies"/> class.
        /// </summary>
        public Currencies()
        {
            var assembly = typeof(Currencies).GetTypeInfo().Assembly;
            var resourceName = assembly.GetManifestResourceNames().Single(x => x.EndsWith("currencies.xml"));
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                this.cache = XDocument.Load(stream);
            }
        }

        /// <summary>
        /// Gets the currency list 
        /// </summary>
        public XDocument CurrencyList => this.cache;
    }
}