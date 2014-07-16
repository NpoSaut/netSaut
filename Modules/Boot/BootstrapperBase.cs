using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace Modules.Boot
{
    public abstract class BootstrapperBase
    {
        protected IUnityContainer Container { get; private set; }

        protected abstract IEnumerable<IModule> EnumerateModules();

        protected virtual IUnityContainer CreateContainer() { return new UnityContainer(); }

        protected virtual void ConfigureContainer()
        {
            Container.RegisterType<IDependencyProvider, AttributeDependencyProvider>();
            Container.RegisterType<IDependenciesResolver, DependenciesResolver>();
        }

        public void Initialize()
        {
            Container = CreateContainer();
            ConfigureContainer();

            var resolver = Container.Resolve<DependenciesResolver>();
            List<IModule> modules = EnumerateModules().ToList();

            foreach (IModule module in modules)
                module.ConfigureContainer(Container);

            foreach (IModule module in resolver.ResolveInitializationOrder(modules))
                module.InitializeModule(Container);
        }

        public void Run() { }
    }
}
