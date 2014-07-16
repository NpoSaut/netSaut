using Microsoft.Practices.Unity;

namespace Modules
{
    public interface IModule
    {
        void ConfigureContainer(IUnityContainer Container);
        void InitializeModule(IUnityContainer Container);
    }
}
