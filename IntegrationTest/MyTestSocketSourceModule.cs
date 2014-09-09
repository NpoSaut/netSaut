using System;
using Communications;
using Communications.Can;
using Microsoft.Practices.Unity;
using Modules;
using Modules.Dependencies;

namespace IntegrationTest
{
    [Provides(typeof (ISocketSource<ICanSocket>))]
    internal class MyTestSocketSourceModule : IModule
    {
        private readonly Func<ISocketSource<ICanSocket>> _socketFactory;
        public MyTestSocketSourceModule(Func<ISocketSource<ICanSocket>> SocketFactory) { _socketFactory = SocketFactory; }

        public void ConfigureContainer(IUnityContainer Container)
        {
            Container.RegisterType<ISocketSource<ICanSocket>>(new InjectionFactory(c => _socketFactory()));
        }

        public void InitializeModule(IUnityContainer Container) { }
    }
}