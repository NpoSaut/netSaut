using System;
using System.Collections.Generic;
using System.Linq;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    public class ConcurrentJournal<TValue> : IJournal<TValue>
    {
        private readonly ILinkedNodesCollectionCleaner<JournalRecord<TValue>> _cleaner;
        private readonly IConcurrentLinkedCollection<JournalRecord<TValue>> _collection;

        public ConcurrentJournal(IConcurrentLinkedCollection<JournalRecord<TValue>> Collection, ILinkedNodesCollectionCleaner<JournalRecord<TValue>> Cleaner)
        {
            _cleaner = Cleaner;
            _collection = Collection;
        }

        /// <summary>Все записи в журнале в порядке устаревания (новые - первыми).</summary>
        public IEnumerable<JournalRecord<TValue>> Records
        {
            get { return _collection.Select(n => n.Item); }
        }

        /// <summary>Добавляет запись в журнал.</summary>
        /// <param name="Value">Значение.</param>
        /// <param name="RecordTime">Время.</param>
        public void AddRecord(TValue Value, DateTime RecordTime) { AddRecord(new JournalRecord<TValue>(RecordTime, Value)); }

        /// <summary>Добавляет запись в журнал.</summary>
        /// <param name="Record">Новая запись для добавления.</param>
        public void AddRecord(JournalRecord<TValue> Record)
        {
            bool insertionSuccessed;
            do
            {
                ConcurrentLogNode<JournalRecord<TValue>> target = _collection.TakeWhile(r => r.Item.Time > Record.Time).LastOrDefault();
                insertionSuccessed = _collection.TryInsert(Record, target);
            } while (!insertionSuccessed);
            _cleaner.Cleanup(_collection);
        }
    }
}
