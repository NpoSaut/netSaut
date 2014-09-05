using System;
using Microsoft.Practices.Unity;
using Modules;
using Modules.Dependencies;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Interpolators;
using Saut.StateModel.Interpolators.InterpolationTools;

namespace Saut.StateModel.Modules
{
    [Provides(typeof (IStateModelService), typeof (IStateProperty))]
    [DependOn(typeof (IStateProperty))]
    public class StateModelModule : IModule
    {
        /// <summary>Конфигурирует контейнер.</summary>
        /// <remarks>Здесь нужно зарегистрировать все типы, предоставляемые этим модулем наружу и используемые им самим.</remarks>
        /// <param name="Container">Конфигурируемый контейнер.</param>
        public void ConfigureContainer(IUnityContainer Container)
        {
            // Внешние сервисы
            Container.RegisterType<IStateModelService, StateModelService>();

            // Интерполяторы
            Container.RegisterType<IWeightingTool<Double>, NumericWeightingTool>();
            Container.RegisterType<IInterpolator<Double>, LinearInterpolator<Double>>();
            Container.RegisterType<IInterpolator<String>, StepInterpolator<String>>();
            Container.RegisterType<IInterpolator<Boolean>, StepInterpolator<Boolean>>();

            // Выборщик
            Container.RegisterType<IRecordPicker, RecordPicker>();

            // Разное
            Container.RegisterType<IDateTimeManager, DateTimeManager>();
        }

        /// <summary>Инициализирует модуль.</summary>
        /// <remarks>Здесь нужно запустить всё, что нужно запустить, создать всё, что нужно создать.</remarks>
        /// <param name="Container">Сконфигурированный контейнер приложения.</param>
        public void InitializeModule(IUnityContainer Container) { }
    }
}
