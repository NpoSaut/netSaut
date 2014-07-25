using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Modules.Dependencies;
using NUnit.Framework;

namespace Modules.Test
{
    [TestFixture]
    public class DependenciesResolverTest
    {
        private interface IService1 { }

        private interface IService2 { }

        private interface IService3 { }

        private abstract class ModuleBase : IModule
        {
            public void ConfigureContainer(IUnityContainer Container) { throw new NotImplementedException(); }
            public void InitializeModule(IUnityContainer Container) { throw new NotImplementedException(); }
        }

        [DependOn(typeof (IService1))]
        private class ModuleD1 : ModuleBase { }

        [Provides(typeof (IService1))]
        private class ModuleP1 : ModuleBase { }

        [Provides(typeof (IService2))]
        private class ModuleP2 : ModuleBase { }

        [DependOn(typeof (IService1))]
        [Provides(typeof (IService2))]
        private class ModuleD1P2 : ModuleBase { }

        [DependOn(typeof (IService2))]
        [Provides(typeof (IService1))]
        private class ModuleD2P1 : ModuleBase { }

        [DependOn(typeof (IService1))]
        [Provides(typeof (IService1))]
        private class ModuleD1P1 : ModuleBase { }

        [DependOn(typeof (IService2))]
        private class ModuleD2 : ModuleBase { }

        private readonly DependenciesResolver _resolver;

        public DependenciesResolverTest() { _resolver = new DependenciesResolver(new ModuleInformationFactory(new AttributeDependencyProvider())); }

        /// <summary>
        ///     Тестирует выброс исключения <see cref="UnresolvableDependenciesException" /> при наличии циклических
        ///     зависимостей
        /// </summary>
        [Test]
        [ExpectedException(typeof (UnresolvableDependenciesException))]
        public void CyclicDependenciesTest()
        {
            var modules = new IModule[] { new ModuleD2P1(), new ModuleD1P2() };
            List<IModule> resolvedModules = _resolver.ResolveInitializationOrder(modules).ToList();
        }

        /// <summary>
        ///     Проверяет, были ли инициализированы оба провайдера сервиса инициализированы перед инициализацией
        ///     модуля-потребителя
        /// </summary>
        [Test]
        public void MultipleProvidersTest()
        {
            var modules = new IModule[] { new ModuleD1(), new ModuleP1(), new ModuleP1() };
            List<IModule> resolvedModules = _resolver.ResolveInitializationOrder(modules).ToList();
            Assert.AreEqual(3, resolvedModules.Count, "Потеряли модуль в процессе разрешения зависимостей");
            Assert.IsInstanceOf<ModuleP1>(resolvedModules[0]);
            Assert.IsInstanceOf<ModuleP1>(resolvedModules[1]);
            Assert.IsInstanceOf<ModuleD1>(resolvedModules[2]);
        }

        /// <summary>
        ///     Тестирует выброс исключения <see cref="UnresolvableDependenciesException" /> при отсутствии провайдера сервиса
        ///     из зависимости
        /// </summary>
        [Test]
        [ExpectedException(typeof (UnresolvableDependenciesException))]
        public void NoProvidersExceptionTest()
        {
            var modules = new IModule[] { new ModuleD1() };
            List<IModule> resolvedModules = _resolver.ResolveInitializationOrder(modules).ToList();
        }

        /// <summary>
        ///     Проверяет правильность очерёдности вызова модулей в случае, если один из модулей имеет зависимость от класса,
        ///     порождаемого им самим
        /// </summary>
        [Test]
        public void SelfDependencyTest()
        {
            var modules = new IModule[] { new ModuleD1P1(), new ModuleP1() };
            List<IModule> resolvedModules = _resolver.ResolveInitializationOrder(modules).ToList();
            Assert.AreEqual(2, resolvedModules.Count, "Потеряли модуль в процессе разрешения зависимостей");
            Assert.IsInstanceOf<ModuleP1>(resolvedModules[0]);
            Assert.IsInstanceOf<ModuleD1P1>(resolvedModules[1]);
        }

        [Test]
        public void SequentialDependency()
        {
            var modules = new IModule[] { new ModuleD2(), new ModuleP2(), new ModuleP1(), new ModuleD1P2() };
            List<IModule> resolvedModules = _resolver.ResolveInitializationOrder(modules).ToList();

            int indexP1 = resolvedModules.FindIndex(m => m is ModuleP1);
            int indexP2 = resolvedModules.FindIndex(m => m is ModuleP2);
            int indexD1P2 = resolvedModules.FindIndex(m => m is ModuleD1P2);
            int indexD2 = resolvedModules.FindIndex(m => m is ModuleD2);

            Assert.LessOrEqual(indexP2, indexD2);
            Assert.LessOrEqual(indexD1P2, indexD2);
            Assert.LessOrEqual(indexP1, indexD1P2);
        }

        [Test]
        public void SimpleDependencyTest()
        {
            var modules = new IModule[] { new ModuleD1(), new ModuleP1() };
            List<IModule> resolvedModules = _resolver.ResolveInitializationOrder(modules).ToList();
            Assert.AreEqual(2, resolvedModules.Count, "Потеряли модуль в процессе разрешения зависимостей");
            Assert.IsInstanceOf<ModuleP1>(resolvedModules[0]);
            Assert.IsInstanceOf<ModuleD1>(resolvedModules[1]);
        }
    }
}
