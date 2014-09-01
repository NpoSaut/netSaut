using System;
using System.Collections.Generic;
using System.Linq;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    public class ConcurrentJournal<TValue> : IJournal<TValue>
    {
        private readonly ILinkedNodesCollectionCleaner<TValue> _cleaner;
        private readonly ConcurrentLinkedNodesCollection<TValue> _records = new ConcurrentLinkedNodesCollection<TValue>();
        public ConcurrentJournal(ILinkedNodesCollectionCleaner<TValue> Cleaner) { _cleaner = Cleaner; }

        /// <summary>Все записи в журнале в порядке устаревания (новые - первыми).</summary>
        public IEnumerable<JournalRecord<TValue>> Records
        {
            get { return _records.Select(r => r.Item); }
        }

        /// <summary>Добавляет запись в журнал.</summary>
        /// <param name="Value">Значение.</param>
        /// <param name="RecordTime">Время.</param>
        public void AddRecord(TValue Value, DateTime RecordTime) { AddRecord(new JournalRecord<TValue>(RecordTime, Value)); }

        /// <summary>Добавляет запись в журнал.</summary>
        /// <param name="Record">Новая запись для добавления.</param>
        public void AddRecord(JournalRecord<TValue> Record)
        {
            var newJournalNode = new ConcurrentLogNode<TValue>(Record);
            bool insertionSuccessed;
            do
            {
                ConcurrentLogNode<TValue> target = _records.TakeWhile(r => r.Item.Time > Record.Time).LastOrDefault();
                insertionSuccessed = _records.TryInsert(newJournalNode, target);
            } while (!insertionSuccessed);
            _cleaner.Cleanup(_records);
        }
    }
}
