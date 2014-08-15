using System;

namespace Saut.StateModel.Interfaces
{
    /// <summary>������ � ������� ��������.</summary>
    /// <typeparam name="TValue">��� ��������.</typeparam>
    public struct JournalRecord<TValue>
    {
        public JournalRecord(DateTime Time, TValue Value) : this()
        {
            this.Time = Time;
            this.Value = Value;
        }

        /// <summary>�����.</summary>
        public DateTime Time { get; private set; }

        /// <summary>��������.</summary>
        public TValue Value { get; private set; }

        public override string ToString() { return string.Format("{0:T} : {1}", Time, Value); }
    }
}
