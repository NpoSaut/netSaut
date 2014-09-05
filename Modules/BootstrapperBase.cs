using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Modules.Dependencies;
using Modules.Exceptions;
using Modules.ModuleRunners;

namespace Modules
{
    public abstract class BootstrapperBase
    {
        private bool _initialized;
        private List<IModule> _modules;
        protected IUnityContainer Container { get; private set; }

        protected abstract IEnumerable<IModule> EnumerateModules();

        protected virtual IUnityContainer CreateContainer() { return new UnityContainer(); }

        protected virtual void ConfigureContainer()
        {
            Container.RegisterType<IDependencyProvider, AttributeDependencyProvider>();
            Container.RegisterType<IDependenciesResolver, DependenciesResolver>();
            Container.RegisterType<IModulesLauncher, ThreadedModulesLauncher>();
        }

        public void Initialize()
        {
            Container = CreateContainer();
            ConfigureContainer();

            var resolver = Container.Resolve<DependenciesResolver>();
            _modules = EnumerateModules().ToList();

            foreach (IModule module in _modules)
                module.ConfigureContainer(Container);

            foreach (IModule module in resolver.ResolveInitializationOrder(_modules))
                module.InitializeModule(Container);

            _initialized = true;
        }

        public void Run()
        {
            if (!_initialized) throw new ModulesWasNotInitializedException();
            var runner = Container.Resolve<IModulesLauncher>();
            runner.RunModules(_modules.OfType<IExecutableModule>().ToList());
        }
    }
}
