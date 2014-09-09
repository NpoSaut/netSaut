using Geographics;
using Microsoft.Practices.Unity;
using Modules;
using Modules.Dependencies;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Interpolators;
using Saut.StateModel.Interpolators.InterpolationTools;
using Saut.StateModel.StateProperties;

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

            // Свойства
            // TODO: ПОЧЕМУ НЕ РАБОТАЕТ ContainerControlledLifetimeManager ???
            Container.RegisterType<IStateProperty, GpsPositionProperty>("GPS Position", new ContainerControlledLifetimeManager());
            Container.RegisterType<IStateProperty, GpsReliabilityProperty>("GPS Reliability", new ContainerControlledLifetimeManager());
            Container.RegisterType<IStateProperty, SpeedProperty>("Speed", new ContainerControlledLifetimeManager());
        }

        /// <summary>Инициализирует модуль</summary>
        /// <remarks>Здесь нужно запустить всё, что нужно запустить, создать всё, что нужно создать.</remarks>
        /// <param name="Container">Сконфигурированный контейнер приложения</param>
        public void InitializeModule(IUnityContainer Container) { }
    }
}
