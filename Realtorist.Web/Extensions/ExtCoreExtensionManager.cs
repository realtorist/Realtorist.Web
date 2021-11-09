using System;
using System.Collections.Generic;
using System.Reflection;
using ExtCore.Infrastructure;
using Realtorist.Extensions.Base.Manager;

namespace Realtorist.Web.Extensions
{
    /// <summary>
    /// Implementation of the extension manager, using ExtCore
    /// </summary>
    public class ExtCoreExtensionManager : IExtensionManager
    {
        /// <inheritdoc/>
        public Type GetImplementation<T>() => ExtensionManager.GetImplementation<T>();

        /// <inheritdoc/>
        public Type GetImplementation<T>(Func<Assembly, bool> predicate) => ExtensionManager.GetImplementation<T>(predicate);

        /// <inheritdoc/>
        public IEnumerable<Type> GetImplementations<T>() => ExtensionManager.GetImplementations<T>();

        /// <inheritdoc/>
        public IEnumerable<Type> GetImplementations<T>(Func<Assembly, bool> predicate) => ExtensionManager.GetImplementations<T>(predicate);

        /// <inheritdoc/>
        public T GetInstance<T>() => ExtensionManager.GetInstance<T>();

        /// <inheritdoc/>
        public T GetInstance<T>(params object[] args) => ExtensionManager.GetInstance<T>(false, args);

        /// <inheritdoc/>
        public T GetInstance<T>(Func<Assembly, bool> predicate) => ExtensionManager.GetInstance<T>(predicate, false);

        /// <inheritdoc/>
        public T GetInstance<T>(Func<Assembly, bool> predicate, params object[] args) => ExtensionManager.GetInstance<T>(predicate, false, args);

        /// <inheritdoc/>
        public IEnumerable<T> GetInstances<T>() => ExtensionManager.GetInstances<T>();

        /// <inheritdoc/>
        public IEnumerable<T> GetInstances<T>(params object[] args) => ExtensionManager.GetInstances<T>(false, args);

        /// <inheritdoc/>
        public IEnumerable<T> GetInstances<T>(Func<Assembly, bool> predicate) => ExtensionManager.GetInstances<T>(predicate, false);

        /// <inheritdoc/>
        public IEnumerable<T> GetInstances<T>(Func<Assembly, bool> predicate, params object[] args) => ExtensionManager.GetInstances<T>(predicate, false, args);
    }
}