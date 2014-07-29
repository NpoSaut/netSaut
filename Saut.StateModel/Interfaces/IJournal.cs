using System;
using System.Collections.Generic;

namespace Saut.StateModel.Interfaces
{
    /// <summary>Журнал изменений значения свойства.</summary>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    public interface IJournal<TValue>
    {
        /// <summary>Все записи в журнале в порядке устаревания (новые - первыми).</summary>
        IEnumerable<JournalRecord<TValue>> Records { get; }

        /// <summary>Добавляет запись в журнал.</summary>
        /// <param name="Value">Значение.</param>
        /// <param name="RecordTime">Время.</param>
        void AddRecord(TValue Value, DateTime RecordTime);
    }
}
