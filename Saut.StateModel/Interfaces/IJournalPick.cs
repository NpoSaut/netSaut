using System.Collections.Generic;

namespace Saut.StateModel.Interfaces
{
    /// <summary>Выборка из журнала.</summary>
    /// <typeparam name="TValue">Тип значений в журнале.</typeparam>
    public interface IJournalPick<TValue>
    {
        /// <summary>Последовательность записей после указанного времени (в порядке первый - ранний).</summary>
        IEnumerable<JournalRecord<TValue>> RecordsAfter { get; }

        /// <summary>Последовательность записей до указанного времени (в порядке первый - поздний).</summary>
        IEnumerable<JournalRecord<TValue>> RecordsBefore { get; }
    }
}
