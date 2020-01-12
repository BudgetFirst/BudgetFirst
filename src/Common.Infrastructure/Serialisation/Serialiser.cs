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

namespace BudgetFirst.Common.Infrastructure.Serialisation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// Provides (data contract) serialisation and de-serialisation
    /// </summary>
    public static class Serialiser
    {
        /// <summary>
        /// Gets or sets known types for serialisation/de-serialisation
        /// </summary>
        public static Type[] KnownTypes { get; set; } = new Type[0];

        /// <summary>
        /// Clone an object via serialisation round trip
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="source">Source object to clone</param>
        /// <returns>Deep clone</returns>
        public static T CloneSerialisable<T>(T source)
        {
            string serialisedObject;

            using (var memoryStream = new MemoryStream())
            {
                Serialise(source, memoryStream);
                memoryStream.Position = 0; // rewind
                var bytes = memoryStream.ToArray();
                serialisedObject = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }

            T roundtrip;

            // Memory stream is disposed when stream writer is disposed
            var writeMemoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(writeMemoryStream))
            {
                streamWriter.Write(serialisedObject);
                streamWriter.Flush();

                // do not close stream yet
                writeMemoryStream.Position = 0; // rewind

                roundtrip = DeSerialise<T>(writeMemoryStream);
            }

            return roundtrip;
        }

        /// <summary>
        /// De-serialise an object from a stream
        /// </summary>
        /// <typeparam name="T">Object type. Must be have <see cref="DataContractAttribute"/>.</typeparam>
        /// <param name="sourceStream">Stream to read from</param>
        /// <returns>De-serialised object</returns>
        public static T DeSerialise<T>(Stream sourceStream)
        {
            DataContractSerializer dataContractSerializer;
            dataContractSerializer = new DataContractSerializer(typeof(T), KnownTypes);
            return (T)dataContractSerializer.ReadObject(sourceStream);
        }

        /// <summary>
        /// De-serialise an object from a stream
        /// </summary>
        /// <typeparam name="T">Object type. Must be have <see cref="DataContractAttribute"/>.</typeparam>
        /// <param name="sourceStream">Stream to read from</param>
        /// <param name="knownTypes">Additional, known types to be supported during de-serialisation.</param>
        /// <returns>De-serialised object</returns>
        public static T DeSerialise<T>(Stream sourceStream, IEnumerable<Type> knownTypes)
        {
            DataContractSerializer dataContractSerializer;
            dataContractSerializer = new DataContractSerializer(typeof(T), knownTypes);
            return (T)dataContractSerializer.ReadObject(sourceStream);
        }

        /// <summary>
        /// Serialise an object onto a stream
        /// </summary>
        /// <param name="object">Object to serialise. Must be have <see cref="DataContractAttribute"/>.</param>
        /// <param name="targetStream">Stream to serialise into</param>
        public static void Serialise(object @object, Stream targetStream)
        {
            var dataContractSerialiser = new DataContractSerializer(@object.GetType(), KnownTypes);
            dataContractSerialiser.WriteObject(targetStream, @object);
        }
    }
}