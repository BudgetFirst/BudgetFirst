// BudgetFirst 
// ©2020 Sabrina Mühlgrabner
//
// This source code is licensed under:
//   * GNU General Public License v3.0 (GPLv3)
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

namespace BudgetFirst.Backend.EventStore
{
    using System;

    /// <summary>
    /// A domain event is no valid to be persisted because some fields are missing.
    /// </summary>
    public class IncompleteEventException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="IncompleteEventException"/> class.
        /// </summary>
        public IncompleteEventException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="IncompleteEventException"/> class.
        /// </summary>
        /// <param name="message">Exception message text</param>
        public IncompleteEventException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="IncompleteEventException"/> class.
        /// </summary>
        /// <param name="message">Exception message text</param>
        /// <param name="innerException">Inner exception</param>
        public IncompleteEventException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}