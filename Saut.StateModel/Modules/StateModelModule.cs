using Microsoft.Practices.Unity;
using Modules;
using Modules.Dependencies;

namespace Saut.StateModel.Modules
{
    [Provides(typeof (IStateModelService), typeof (IStateProperty))]
    [DependOn(typeof (IStateProperty))]
    public class StateModelModule : IModule
    {
        /// <summary>Конфигурирует контейнер.</summary>
        /// <remarks>Здесь нужно зарегистрировать все типы, предоставляемые этим модулем наружу и используемые им самим.</remarks>
        /// <param name="Container">Конфигурируемый контейнер.</param>
        public void ConfigureContainer(IUnityContainer Container) { Container.RegisterType<IStateModelService, StateModelService>(); }

        /// <summary>Инициализирует модуль.</summary>
        /// <remarks>Здесь нужно запустить всё, что нужно запустить, создать всё, что нужно создать.</remarks>
        /// <param name="Container">Сконфигурированный контейнер приложения.</param>
        public void InitializeModule(IUnityContainer Container) { }
    }
}
