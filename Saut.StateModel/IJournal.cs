using System;
using System.Collections.Generic;

namespace Saut.StateModel
{
    public interface IJournal<TValue>
    {
        IEnumerable<JournalRecord<TValue>> Records { get; }
        void AddRecord(TValue Record, DateTime RecordTime);
    }

    public struct JournalRecord<TValue>
    {
        public JournalRecord(DateTime Time, TValue Value) : this()
        {
            this.Time = Time;
            this.Value = Value;
        }

        public DateTime Time { get; private set; }
        public TValue Value { get; private set; }
    }
}