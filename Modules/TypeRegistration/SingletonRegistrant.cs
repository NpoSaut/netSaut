using System;
using Microsoft.Practices.Unity;

namespace Modules.TypeRegistration
{
    public class SingletonRegistrant : IRegistrant
    {
        public void RegisterAll(ITypesEnumerator TypesEnumerator, IUnityContainer Container)
        {
            foreach (Type type in TypesEnumerator.EnumerateTypes())
                Container.RegisterType(type, new ContainerControlledLifetimeManager());
        }
    }
}
