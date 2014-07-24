using Microsoft.Practices.Unity;

namespace Modules
{
    /// <summary>Модуль программы</summary>
    public interface IModule
    {
        /// <summary>Конфигурирует контейнер</summary>
        /// <remarks>Здесь нужно зарегистрировать все типы, предоставляемые этим модулем наружу и используемые им самим</remarks>
        /// <param name="Container">Конфигурируемый контейнер</param>
        void ConfigureContainer(IUnityContainer Container);

        /// <summary>Инициализирует модуль</summary>
        /// <remarks>Здесь нужно запустить всё, что нужно запустить, создать всё, что нужно создать.</remarks>
        /// <param name="Container">Сконфигурированный контейнер приложения</param>
        void InitializeModule(IUnityContainer Container);
    }
}
