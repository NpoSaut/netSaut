using System;

namespace Saut.StateModel.Interfaces
{
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

        public override string ToString() { return string.Format("{0:T} : {1}", Time, Value); }
    }
}
