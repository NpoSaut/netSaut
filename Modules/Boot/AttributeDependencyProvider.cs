using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Dependencies;

namespace Modules.Boot
{
    public interface IDependencyProvider
    {
        IList<Type> GetProvides(IModule Module);
        IList<Type> GetProvides<TModule>() where TModule : IModule;
        IList<Type> GetProvides(Type ModuleType);
        IList<Type> GetDependencies(IModule Module);
        IList<Type> GetDependencies<TModule>() where TModule : IModule;
        IList<Type> GetDependencies(Type ModuleType);
    }

    public class AttributeDependencyProvider : IDependencyProvider
    {
        public IList<Type> GetProvides(IModule Module) { return GetProvides(Module.GetType()); }
        public IList<Type> GetProvides<TModule>() where TModule : IModule { return GetProvides(typeof (TModule)); }

        public IList<Type> GetProvides(Type ModuleType)
        {
            return Attribute.GetCustomAttributes(ModuleType).OfType<ProvidesAttribute>().SelectMany(a => a.ProvidedServices).ToList();
        }

        public IList<Type> GetDependencies(IModule Module) { return GetDependencies(Module.GetType()); }
        public IList<Type> GetDependencies<TModule>() where TModule : IModule { return GetDependencies(typeof (TModule)); }

        public IList<Type> GetDependencies(Type ModuleType)
        {
            return Attribute.GetCustomAttributes(ModuleType).OfType<DependOnAttribute>().SelectMany(a => a.Dependencies).ToList();
        }
    }
}
