using Microsoft.Practices.Unity;
using Modules;
using Modules.Dependencies;

namespace Saut.FrameProcessing.Modules
{
    [DependOn(typeof (IFrameProcessor))]
    [Provides(typeof (IFrameProcessingService))]
    public class FrameProcessingModule : IModule
    {
        /// <summary>Конфигурирует контейнер</summary>
        /// <remarks>Здесь нужно зарегистрировать все типы, предоставляемые этим модулем наружу и используемые им самим</remarks>
        /// <param name="Container">Конфигурируемый контейнер</param>
        public void ConfigureContainer(IUnityContainer Container) { Container.RegisterType<IFrameProcessingService, FrameProcessingService>(); }

        /// <summary>Инициализирует модуль</summary>
        /// <remarks>Здесь нужно запустить всё, что нужно запустить, создать всё, что нужно создать.</remarks>
        /// <param name="Container">Сконфигурированный контейнер приложения</param>
        public void InitializeModule(IUnityContainer Container) { throw new System.NotImplementedException(); }
    }
}