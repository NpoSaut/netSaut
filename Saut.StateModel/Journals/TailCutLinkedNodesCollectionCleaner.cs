using System;
using System.Collections.Generic;
using System.Linq;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    /// <summary>Инструмент по отчистке коллекции, отрезающий хвост за первым не актуальным элементом</summary>
    /// <typeparam name="TCollectionElementValue">Тип значения записи журнала</typeparam>
    public class TailCutLinkedNodesCollectionCleaner<TCollectionElementValue> : ILinkedNodesCollectionCleaner<TCollectionElementValue>
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
            ConcurrentLogNode<TCollectionElementValue> lastActualElement = Records.FirstOrDefault(el => el.Item.Time <= actualityTime);
            CutNext(lastActualElement);
        }

        /// <summary>Обрезает цепочку после указанного элемента</summary>
        /// <remarks>
        ///     После некоторых размышлений, пришёл к выводу, что не нужно производить рекурсивное разрушение цепочки, т.к.
        ///     сборщик мусора наверняка достаточно умный, чтобы понять, что все отрезанные объекты можно изничтожить, если на них
        ///     нет ссылки с головы.
        /// </remarks>
        /// <param name="Element">Элемент, после которого цепочка будет разорвана</param>
        private void CutNext(ConcurrentLogNode<TCollectionElementValue> Element)
        {
            if (Element == null) return;
            Element.Next = null;
        }
    }
}
