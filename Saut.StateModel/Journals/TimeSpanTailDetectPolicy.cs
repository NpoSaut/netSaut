using System;
using System.Collections.Generic;
using System.Linq;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    /// <summary>Политика выбора последнего актуального элемента на основании устаревания записи по времени</summary>
    /// <typeparam name="TJournalElementValue">Тип значения в записи журнала</typeparam>
    public class TimeSpanTailDetectPolicy<TJournalElementValue> : ITailDetectPolicy<JournalRecord<TJournalElementValue>>
    {
        private readonly TimeSpan _actualityTimeSpan;
        private readonly IDateTimeManager _dateTimeManager;

        /// <summary>
        ///     Создаёт новый экземпляр политики выбора последнего актуального элемента на основании устаревания записи по
        ///     времени
        /// </summary>
        /// <param name="ActualityTimeSpan">Временной промежуток, в течении которого значение в журнале считается актуальным</param>
        /// <param name="DateTimeManager">Менеджер текущего времени</param>
        public TimeSpanTailDetectPolicy(TimeSpan ActualityTimeSpan, IDateTimeManager DateTimeManager)
        {
            _actualityTimeSpan = ActualityTimeSpan;
            _dateTimeManager = DateTimeManager;
        }

        /// <summary>Ищет последний актуальный элемент в коллекции</summary>
        /// <param name="Records">Коллекция для усечения</param>
        /// <returns>Элемент, после которого можно производить усечение коллекции</returns>
        public ConcurrentLogNode<JournalRecord<TJournalElementValue>> GetLastActualElement(
            IEnumerable<ConcurrentLogNode<JournalRecord<TJournalElementValue>>> Records)
        {
            DateTime actualityTime = _dateTimeManager.Now - _actualityTimeSpan;
            return Records.FirstOrDefault(el => el.Item.Time <= actualityTime);
        }
    }
}
