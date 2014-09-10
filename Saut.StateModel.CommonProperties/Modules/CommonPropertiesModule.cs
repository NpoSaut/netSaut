using System.Reflection;
using Geographics;
using Microsoft.Practices.Unity;
using Modules;
using Modules.Dependencies;
using Modules.TypeRegistration;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Interpolators;
using Saut.StateModel.Interpolators.InterpolationTools;

namespace Saut.StateModel.Modules
{
    [Provides(typeof (IStateProperty))]
    public class CommonPropertiesModule : IModule
    {
        /// <summary>Конфигурирует контейнер</summary>
        /// <remarks>Здесь нужно зарегистрировать все типы, предоставляемые этим модулем наружу и используемые им самим</remarks>
        /// <param name="Container">Конфигурируемый контейнер</param>
        public void ConfigureContainer(IUnityContainer Container)
        {
            // Интерполяторы
            Container.RegisterType<IWeightingTool<EarthPoint>, EarthPointWeightingTool>();
            Container.RegisterType<IInterpolator<EarthPoint>, LinearInterpolator<EarthPoint>>();
        }

        /// <summary>Инициализирует модуль</summary>
        /// <remarks>Здесь нужно запустить всё, что нужно запустить, создать всё, что нужно создать.</remarks>
        /// <param name="Container">Сконфигурированный контейнер приложения</param>
        public void InitializeModule(IUnityContainer Container)
        {
            var enumerator = new AssemblyScanTypesEnumerator(Assembly.GetAssembly(typeof (CommonPropertiesModule)), typeof (IStateProperty));
            var registrant = Container.Resolve<IRegistrant>("PropertyRegistrant");
            registrant.RegisterAll(enumerator, Container);
        }
    }
}
