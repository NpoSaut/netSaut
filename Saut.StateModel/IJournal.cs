using System;
using System.Collections.Generic;

namespace Saut.StateModel
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

    /// <summary>Запись в журнале значений.</summary>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    public struct JournalRecord<TValue>
    {
        public JournalRecord(DateTime Time, TValue Value) : this()
        {
            this.Time = Time;
            this.Value = Value;
        }

        /// <summary>Время.</summary>
        public DateTime Time { get; private set; }

        /// <summary>Значение.</summary>
        public TValue Value { get; private set; }
    }
}
