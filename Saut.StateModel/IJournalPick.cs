using System.Collections.Generic;

namespace Saut.StateModel
{
    public interface IJournalPick<TValue>
    {
        IEnumerable<JournalRecord<TValue>> RecordsAfter { get; }
        IEnumerable<JournalRecord<TValue>> RecordsBefore { get; }
    }
}