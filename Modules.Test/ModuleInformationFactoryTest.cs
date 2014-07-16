using System;
using Microsoft.Practices.Unity;
using Modules.Dependencies;
using NUnit.Framework;
using Rhino.Mocks;

namespace Modules.Test
{
    [TestFixture]
    public class ModuleInformationFactoryTest
    {
        private class Module : IModule
        {
            public void ConfigureContainer(IUnityContainer Container) { throw new NotImplementedException(); }
            public void InitializeModule(IUnityContainer Container) { throw new NotImplementedException(); }
        }

        private static IDependencyProvider GetDependencyProviderStrub(IModule m1, Type[] dependencies, Type[] products)
        {
            var dependencyProviderStrub = MockRepository.GenerateMock<IDependencyProvider>();

            dependencyProviderStrub.Stub(m => m.GetDependencies<Module>()).Return(dependencies);
            dependencyProviderStrub.Stub(m => m.GetDependencies(typeof (Module))).Return(dependencies);
            dependencyProviderStrub.Stub(m => m.GetDependencies(m1)).Return(dependencies);

            dependencyProviderStrub.Stub(m => m.GetProvides<Module>()).Return(products);
            dependencyProviderStrub.Stub(m => m.GetProvides(typeof (Module))).Return(products);
            dependencyProviderStrub.Stub(m => m.GetProvides(m1)).Return(products);
            return dependencyProviderStrub;
        }

        [Test]
        public void TestGetModuleInformation()
        {
            var products = new Type[0];
            var dependencies = new Type[0];

            IModule m1 = new Module();

            IDependencyProvider dependencyProviderStrub = GetDependencyProviderStrub(m1, dependencies, products);

            var factory = new ModuleInformationFactory(dependencyProviderStrub);
            ModuleInformation mi = factory.GetModuleInformation(m1);

            Assert.AreSame(m1, mi.Module);
            Assert.AreSame(dependencies, mi.Dependencies);
            Assert.AreSame(products, mi.Provides);
        }
    }
}
