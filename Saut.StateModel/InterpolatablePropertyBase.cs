﻿using System;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Obsoleting;

namespace Saut.StateModel
{
    /// <summary>Интерполируемое журналируемое свойство состояния.</summary>
    /// <typeparam name="TValue">Тип значения свойства.</typeparam>
    [ObsoleteTimeout(2000)]
    public abstract class InterpolatablePropertyBase<TValue> : IJournaledStateProperty<TValue>
    {
        protected readonly IJournal<TValue> Journal;
        private readonly IInterpolator<TValue> _interpolator;
        private readonly IObsoletePolicy _obsoletePolicy;
        private readonly IRecordPicker _recordPicker;
        private readonly IDateTimeManager _timeManager;

        public InterpolatablePropertyBase(IDateTimeManager TimeManager, IJournalFactory<TValue> JournalFactory, IInterpolator<TValue> Interpolator,
                                          IRecordPicker RecordPicker, IObsoletePolicyProvider ObsoletePolicyProvider)
        {
            _timeManager = TimeManager;
            _interpolator = Interpolator;
            _recordPicker = RecordPicker;
            Journal = JournalFactory.GetJournal();
            _obsoletePolicy = ObsoletePolicyProvider.GetObsoletePolicy(this);
        }

        /// <summary>Название свойства.</summary>
        public abstract string Name { get; }

        /// <summary>Показывает, задано ли значение для свойства в указанный момент времени</summary>
        /// <param name="OnTime">Момент времени</param>
        /// <returns>True, если значение задано</returns>
        public bool HaveValue(DateTime OnTime)
        {
            IJournalPick<TValue> pick = _recordPicker.PickRecords(Journal, OnTime);
            pick = _obsoletePolicy.DecoratePick(pick, OnTime);
            return _interpolator.CanInterpolate(pick, OnTime);
        }

        /// <summary>Получает значение свойства в указанный момент времени.</summary>
        /// <param name="OnTime">Момент времени.</param>
        /// <returns>Значение свойства в указанный момент времени.</returns>
        public TValue GetValue(DateTime OnTime)
        {
            IJournalPick<TValue> pick = _recordPicker.PickRecords(Journal, OnTime);
            pick = _obsoletePolicy.DecoratePick(pick, OnTime);
            TValue value = _interpolator.Interpolate(pick, OnTime);
            return value;
        }

        /// <summary>Устанавливает новое значение свойства.</summary>
        /// <param name="NewValue">Новое значение свойства.</param>
        /// <param name="OnTime">Момент актуализации значения.</param>
        public void UpdateValue(TValue NewValue, DateTime OnTime) { Journal.AddRecord(NewValue, OnTime); }

        #region Перегрузки для нежурналиуемых методов

        /// <summary>Показывает, задано ли значение для свойства</summary>
        /// <returns>True, если значение задано</returns>
        public bool HaveValue() { return HaveValue(_timeManager.Now); }

        /// <summary>Получает текущее значение свойства.</summary>
        /// <returns>Значение свойства на момент запроса.</returns>
        public TValue GetValue() { return GetValue(_timeManager.Now); }

        /// <summary>Устанавливает новое значение свойства.</summary>
        /// <param name="NewValue">Новое значение свойства.</param>
        public void UpdateValue(TValue NewValue) { UpdateValue(NewValue, _timeManager.Now); }

        #endregion
    }
}
