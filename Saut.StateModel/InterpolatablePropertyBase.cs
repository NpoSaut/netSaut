﻿using System;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel
{
    /// <summary>Интерполируемое журналируемое свойство состояния.</summary>
    /// <typeparam name="TValue">Тип значения свойства.</typeparam>
    public abstract class InterpolatablePropertyBase<TValue> : IJournaledStateProperty<TValue>
    {
        protected readonly IJournal<TValue> Journal;
        private readonly IInterpolator<TValue> _interpolator;
        private readonly IRecordPicker _recordPicker;
        private readonly IDateTimeManager _timeManager;

        public InterpolatablePropertyBase(IDateTimeManager TimeManager, IJournal<TValue> Journal, IInterpolator<TValue> Interpolator, IRecordPicker RecordPicker)
        {
            _timeManager = TimeManager;
            _interpolator = Interpolator;
            _recordPicker = RecordPicker;
            this.Journal = Journal;
        }

        /// <summary>Название свойства.</summary>
        public abstract string Name { get; }

        /// <summary>Получает значение свойства в указанный момент времени.</summary>
        /// <param name="OnTime">Момент времени.</param>
        /// <returns>Значение свойства в указанный момент времени.</returns>
        public TValue GetValue(DateTime OnTime)
        {
            IJournalPick<TValue> pick = _recordPicker.PickRecords(Journal, OnTime);
            TValue value = _interpolator.Interpolate(pick, OnTime);
            return value;
        }

        /// <summary>Устанавливает новое значение свойства.</summary>
        /// <param name="NewValue">Новое значение свойства.</param>
        /// <param name="OnTime">Момент актуализации значения.</param>
        public void UpdateValue(TValue NewValue, DateTime OnTime) { Journal.AddRecord(NewValue, OnTime); }

        #region Перегрузки для нежурналиуемых методов

        /// <summary>Получает текущее значение свойства.</summary>
        /// <returns>Значение свойства на момент запроса.</returns>
        public TValue GetValue() { return GetValue(_timeManager.Now); }

        /// <summary>Устанавливает новое значение свойства.</summary>
        /// <param name="NewValue">Новое значение свойства.</param>
        public void UpdateValue(TValue NewValue) { UpdateValue(NewValue, _timeManager.Now); }

        #endregion
    }
}