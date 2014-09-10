using Microsoft.Practices.Unity;

namespace Modules.TypeRegistration
{
    public interface IRegistrant
    {
        void RegisterAll(ITypesEnumerator TypesEnumerator, IUnityContainer Container);
    }
}