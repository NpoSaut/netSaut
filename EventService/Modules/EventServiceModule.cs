using EventService.ConsumerContainers;
using EventService.Interfaces;
using Microsoft.Practices.Unity;
using Modules;
using Modules.Dependencies;
using Saut.EventServices;

namespace EventService.Modules
{
    [Provides(typeof (IEventAggregator))]
    public class EventServiceModule : IModule
    {
        /// <summary>Конфигурирует контейнер</summary>
        /// <remarks>Здесь нужно зарегистрировать все типы, предоставляемые этим модулем наружу и используемые им самим</remarks>
        /// <param name="Container">Конфигурируемый контейнер</param>
        public void ConfigureContainer(IUnityContainer Container)
        {
            Container.RegisterType<IEventExpectantFactory, ConcurrentEventExpectantFactory>();
            Container.RegisterType<IEventListenerFactory, DirectEventListenerFactory>();

            Container.RegisterType<IConsumersContainer, LockFreeListConsumersContainer>();
            Container.RegisterType<IEventAggregator, EventAggregator>();
        }

        /// <summary>Инициализирует модуль</summary>
        /// <remarks>Здесь нужно запустить всё, что нужно запустить, создать всё, что нужно создать.</remarks>
        /// <param name="Container">Сконфигурированный контейнер приложения</param>
        public void InitializeModule(IUnityContainer Container) { }
    }
}
