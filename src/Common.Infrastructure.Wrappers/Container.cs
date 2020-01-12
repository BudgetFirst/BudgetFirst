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

namespace BudgetFirst.Common.Infrastructure.Wrappers
{
    using System;
    using System.Collections.Generic;

    using BudgetFirst.Common.Infrastructure.DependencyInjection;

    /// <summary>
    /// A wrapper around the SimpleInjector container.
    /// </summary>
    public sealed class Container : IContainer, IDisposable
    {
        /// <summary>
        /// The underlying container.
        /// </summary>
        private readonly SimpleInjector.Container container;

        /// <summary>
        /// Initialises a new instance of the <see cref="Container"/> class.
        /// </summary>
        public Container()
        {
            this.container = new SimpleInjector.Container();
        }

        /// <summary>
        /// The caching method of the registration within the container.
        /// </summary>
        public enum Lifestyle
        {
            /// <summary>
            /// <para>
            /// The lifestyle that caches components during the lifetime of the <see cref="Container"/> instance
            /// and guarantees that only a single instance of that component is created for that instance. Since
            /// general use is to create a single <b>Container</b> instance for the lifetime of the application /
            /// AppDomain, this would mean that only a single instance of that component would exist during the
            /// lifetime of the application. In a multi-threaded applications, implementations registered using 
            /// this lifestyle must be thread-safe.
            /// </para>
            /// <para>
            /// In case the type of a cached instance implements <see cref="IDisposable"/>, the container will
            /// ensure its disposal when the container gets disposed.
            /// </para>
            /// </summary>
            Singleton,

            /// <summary>
            /// The lifestyle instance that doesn't cache instances. A new instance of the specified
            /// component is created every time the registered service it is requested or injected.
            /// </summary>
            Transient
        }

        /// <summary>
        /// Registers that an instance of <typeparamref name="TImplementation"/> will be returned when an
        /// instance of type <typeparamref name="TService"/> is requested. The instance is cached according to 
        /// the supplied <paramref name="lifestyle"/>.
        /// </summary>
        /// <typeparam name="TService">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <typeparam name="TImplementation">The concrete type that will be registered.</typeparam>
        /// <param name="lifestyle">The lifestyle that specifies how the returned instance will be cached.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when this container instance is locked and can not be altered, or when an 
        /// the <typeparamref name="TService"/> has already been registered.</exception>
        /// <exception cref="ArgumentException">Thrown when the given <typeparamref name="TImplementation"/> 
        /// type is not a type that can be created by the container.
        /// </exception>
        public void Register<TService, TImplementation>(Lifestyle lifestyle) where TService : class
            where TImplementation : class, TService
        {
            this.container.Register<TService, TImplementation>(this.ConvertLifestyle(lifestyle));
        }

        /// <summary>
        /// Registers that a new instance of <typeparamref name="TImplementation"/> will be returned every time a
        /// <typeparamref name="TService"/> is requested (transient).
        /// </summary>
        /// <typeparam name="TService">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <typeparam name="TImplementation">The concrete type that will be registered.</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Thrown when this container instance is locked and can not be altered, or when an 
        /// the <typeparamref name="TService"/> has already been registered.</exception>
        /// <exception cref="ArgumentException">Thrown when the given <typeparamref name="TImplementation"/> 
        /// type is not a type that can be created by the container.
        /// </exception>
        public void Register<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            this.container.Register<TService, TImplementation>();
        }

        /// <summary>
        /// Registers that an  instance of <typeparamref name="TConcrete"/> will be returned when it 
        /// is requested. The instance is cached according to the supplied <paramref name="lifestyle"/>.
        /// </summary>
        /// <typeparam name="TConcrete">The concrete type that will be registered.</typeparam>
        /// <param name="lifestyle">The lifestyle that specifies how the returned instance will be cached.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when this container instance is locked and can not be altered, or when an 
        /// the <typeparamref name="TConcrete"/> has already been registered.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown when the <typeparamref name="TConcrete"/> is a type
        /// that can not be created by the container.</exception>
        public void Register<TConcrete>(Lifestyle lifestyle) where TConcrete : class
        {
            this.container.Register<TConcrete>(this.ConvertLifestyle(lifestyle));
        }

        /// <summary>
        /// Registers a single instance that will be returned when an instance of type 
        /// <typeparamref name="TService"/> is requested. This <paramref name="instance"/> must be thread-safe
        /// when working in a multi-threaded environment.
        /// <b>NOTE:</b> Do note that instances supplied by this method <b>NEVER</b> get disposed by the
        /// container, since the instance is assumed to outlive this container instance. If disposing is
        /// required, use the overload that accepts a <see cref="Func{T}"/> delegate.
        /// </summary>
        /// <typeparam name="TService">The interface or base type that can be used to retrieve the instance.</typeparam>
        /// <param name="instance">The instance to register.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when this container instance is locked and can not be altered, or when the 
        /// <typeparamref name="TService"/> has already been registered.</exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="instance"/> is a null reference.
        /// </exception>
        public void RegisterSingleton<TService>(TService instance) where TService : class
        {
            this.container.RegisterSingleton<TService>(instance);
        }

        /// <summary>
        /// Resolve an instance from the container
        /// </summary>
        /// <typeparam name="TInstance">Type of instance to resolve</typeparam>
        /// <returns>Resolved instance</returns>
        public TInstance Resolve<TInstance>() where TInstance : class
        {
            return this.container.GetInstance<TInstance>();
        }

        /// <summary>
        /// Resolve an instance from the container
        /// </summary>
        /// <param name="instanceType">Type of instance to resolve</param>
        /// <returns>Resolved instance</returns>
        public object Resolve(Type instanceType)
        {
            return this.container.GetInstance(instanceType);
        }

        /// <summary>
        /// Resolve all registered instances for the given type
        /// </summary>
        /// <typeparam name="TInstance">Type of instance to resolve</typeparam>
        /// <returns>All registered instances for the given type</returns>
        public IEnumerable<TInstance> ResolveAll<TInstance>() where TInstance : class
        {
            return this.container.GetAllInstances<TInstance>();
        }

        /// <summary>
        /// Verifies and diagnoses this <b>Container</b> instance. This method will call all registered 
        /// delegates, iterate registered collections and throws an exception if there was an error.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the registration of instances was
        /// invalid.</exception>
        public void Verify()
        {
            this.container.Verify();
        }

        /// <summary>
        /// Dispose of the instance.
        /// </summary>
        public void Dispose()
        {
            this.container.Dispose();
        }

        /// <summary>
        /// Converts between the local Lifestyle enum and the SimpleInjector equivalent.
        /// </summary>
        /// <param name="lifestyle">The Lifestyle value</param>
        /// <returns>The simple injector lifestyle equivalent.</returns>
        private SimpleInjector.Lifestyle ConvertLifestyle(Lifestyle lifestyle)
        {
            switch (lifestyle)
            {
                case Lifestyle.Singleton:
                    return SimpleInjector.Lifestyle.Singleton;
                case Lifestyle.Transient:
                    return SimpleInjector.Lifestyle.Transient;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}