using System;
using System.Collections.Generic;

namespace Modules.Dependencies
{
    public class ModuleInformation
    {
        public ModuleInformation(IModule Module, IList<Type> Dependencies, IList<Type> Provides)
        {
            this.Module = Module;
            this.Dependencies = Dependencies;
            this.Provides = Provides;
        }

        public IModule Module { get; private set; }
        public IList<Type> Dependencies { get; private set; }
        public IList<Type> Provides { get; private set; }
    }

    public class ModuleInformationFactory
    {
        private readonly IDependencyProvider _dependencyProvider;
        public ModuleInformationFactory(IDependencyProvider DependencyProvider) { _dependencyProvider = DependencyProvider; }

        public ModuleInformation GetModuleInformation(IModule Module)
        {
            return
                new ModuleInformation(
                    Module,
                    _dependencyProvider.GetDependencies(Module),
                    _dependencyProvider.GetProvides(Module));
        }
    }
}
