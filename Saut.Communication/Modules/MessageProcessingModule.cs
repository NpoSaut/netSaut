using Communications;
using Communications.Can;
using Microsoft.Practices.Unity;
using Modules;
using Modules.Dependencies;
using Saut.Communication.Interfaces;
using Saut.Communication.ProcessingServices;

namespace Saut.Communication.Modules
{
    /// <summary>Сервис обработки входящих сообщений</summary>
    [DependOn(typeof (IMessageProcessor), typeof (ISocketSource<ICanSocket>))]
    [Provides(typeof (IMessageProcessingService))]
    public class MessageProcessingModule : IExecutableModule
    {
        private IMessageProcessingService _service;

        /// <summary>Конфигурирует контейнер</summary>
        /// <remarks>Здесь нужно зарегистрировать все типы, предоставляемые этим модулем наружу и используемые им самим</remarks>
        /// <param name="Container">Конфигурируемый контейнер</param>
        public void ConfigureContainer(IUnityContainer Container)
        {
            Container.RegisterType<IDeliveryGuy, BasicDeliveryGuy>();
            Container.RegisterType<IMessageProcessingService, MessageProcessingService>(new ContainerControlledLifetimeManager());
        }

        /// <summary>Инициализирует модуль</summary>
        /// <remarks>Здесь нужно запустить всё, что нужно запустить, создать всё, что нужно создать.</remarks>
        /// <param name="Container">Сконфигурированный контейнер приложения</param>
        public void InitializeModule(IUnityContainer Container) { _service = Container.Resolve<IMessageProcessingService>(); }

        public void Run() { _service.Run(); }
    }
}
