using System;
using System.Collections.Generic;
using System.Linq;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    /// <summary>Инструмент по отчистке коллекции записей в журнале от неактуальных элементов</summary>
    /// <typeparam name="TCollectionElementValue">Тип значения записи журнала</typeparam>
    public interface ILinkedNodesCollectionCleaner<TCollectionElementValue>
    {
        /// <summary>Отчищает коллекцию записей от устаревших элементов</summary>
        /// <param name="Records">Коллекция записей</param>
        void Cleanup(IEnumerable<ConcurrentLogNode<TCollectionElementValue>> Records);
    }

    /// <summary>Инструмент по отчистке коллекции, отрезающий хвост за первым не актуальным элементом</summary>
    /// <typeparam name="TCollectionElementValue">Тип значения записи журнала</typeparam>
    internal class TailCutLinkedNodesCollectionCleaner<TCollectionElementValue> : ILinkedNodesCollectionCleaner<TCollectionElementValue>
    {
        private readonly TimeSpan _actualityTimeSpan;
        private readonly IDateTimeManager _dateTimeManager;

        public TailCutLinkedNodesCollectionCleaner(TimeSpan ActualityTimeSpan, IDateTimeManager DateTimeManager)
        {
            _actualityTimeSpan = ActualityTimeSpan;
            _dateTimeManager = DateTimeManager;
        }

        /// <summary>Отчищает коллекцию записей от устаревших элементов</summary>
        /// <param name="Records">Коллекция записей</param>
        public void Cleanup(IEnumerable<ConcurrentLogNode<TCollectionElementValue>> Records)
        {
            DateTime actualityTime = _dateTimeManager.Now - _actualityTimeSpan;
            ConcurrentLogNode<TCollectionElementValue> cutOffElement = Records.FirstOrDefault(el => el.Item.Time <= actualityTime);
            if (cutOffElement != null)
                cutOffElement.Next = null;
        }
    }
}
