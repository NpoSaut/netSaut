﻿using System;
using Microsoft.Practices.Unity;
using Modules;
using Modules.Dependencies;
using Modules.TypeRegistration;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Interpolators;
using Saut.StateModel.Interpolators.InterpolationTools;
using Saut.StateModel.Journals;
using Saut.StateModel.Obsoleting;

namespace Saut.StateModel.Modules
{
    public class StateModelModule : IModule
    {
        /// <summary>Конфигурирует контейнер.</summary>
        /// <remarks>Здесь нужно зарегистрировать все типы, предоставляемые этим модулем наружу и используемые им самим.</remarks>
        /// <param name="Container">Конфигурируемый контейнер.</param>
        public void ConfigureContainer(IUnityContainer Container)
        {
            // Интерполяторы
            Container.RegisterType<IWeightingTool<Double>, NumericWeightingTool>();
            Container.RegisterType<IInterpolator<Double>, LinearInterpolator<Double>>();
            Container.RegisterType<IInterpolator<String>, StepInterpolator<String>>();
            Container.RegisterType<IInterpolator<Boolean>, StepInterpolator<Boolean>>();

            // Журнал
            Container.RegisterType(typeof (IJournalFactory<>), typeof (TimeCuttedConcurrentJournalFactory<>));

            // Выборщик
            Container.RegisterType<IRecordPicker, RecordPicker>();

            // Разное
            Container.RegisterType<IDateTimeManager, DateTimeManager>();
            Container.RegisterType<IRegistrant, SingletonRegistrant>("PropertyRegistrant");
            Container.RegisterType<IObsoletePolicyProvider, TimeoutAttributeObsoletePolicyProvider>();
        }

        /// <summary>Инициализирует модуль.</summary>
        /// <remarks>Здесь нужно запустить всё, что нужно запустить, создать всё, что нужно создать.</remarks>
        /// <param name="Container">Сконфигурированный контейнер приложения.</param>
        public void InitializeModule(IUnityContainer Container) { }
    }
}
