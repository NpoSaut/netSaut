using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Modules.Dependencies;
using NUnit.Framework;

namespace Modules.Test
{
    [TestFixture]
    public class AttributeDependencyProviderTest
    {
        private interface IDependency1 { }

        private interface IDependency2 { }

        private interface IProduct1 { }

        private interface IProduct2 { }

        [DependOn(typeof (IDependency1), typeof (IDependency2))]
        [Provides(typeof (IProduct1), typeof (IProduct2))]
        private class TestModule : IModule
        {
            public void ConfigureContainer(IUnityContainer Container) { throw new NotImplementedException(); }
            public void InitializeModule(IUnityContainer Container) { throw new NotImplementedException(); }
        }

        public AttributeDependencyProviderTest() { _provider = new AttributeDependencyProvider(); }

        private void TestDependencies(IList<Type> Dependencies)
        {
            Assert.Contains(typeof (IDependency1), (ICollection)Dependencies);
            Assert.Contains(typeof (IDependency2), (ICollection)Dependencies);
        }

        private void TestProvides(IList<Type> Products)
        {
            Assert.Contains(typeof (IProduct1), (ICollection)Products);
            Assert.Contains(typeof (IProduct2), (ICollection)Products);
        }

        private readonly AttributeDependencyProvider _provider;

        [Test]
        public void TestGenericDependencies()
        {
            IList<Type> products = _provider.GetDependencies<TestModule>();
            TestDependencies(products);
        }

        [Test]
        public void TestGenericProduces()
        {
            IList<Type> products = _provider.GetProvides<TestModule>();
            TestProvides(products);
        }

        [Test]
        public void TestObjectDependencies()
        {
            IList<Type> products = _provider.GetDependencies(new TestModule());
            TestDependencies(products);
        }

        [Test]
        public void TestObjectProduces()
        {
            IList<Type> products = _provider.GetProvides(new TestModule());
            TestProvides(products);
        }

        [Test]
        public void TestTypeDependencies()
        {
            IList<Type> products = _provider.GetDependencies(typeof (TestModule));
            TestDependencies(products);
        }

        [Test]
        public void TestTypeProduces()
        {
            IList<Type> products = _provider.GetProvides(typeof (TestModule));
            TestProvides(products);
        }
    }
}
