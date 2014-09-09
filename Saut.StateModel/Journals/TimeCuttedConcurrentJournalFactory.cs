using System;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    internal class TimeCuttedConcurrentJournalFactory<TJournalValue> : IJournalFactory<TJournalValue>
    {
        private readonly IDateTimeManager _dateTimeManager;
        public TimeCuttedConcurrentJournalFactory(IDateTimeManager DateTimeManager) { _dateTimeManager = DateTimeManager; }

        public IJournal<TJournalValue> GetJournal()
        {
            return new ConcurrentJournal<TJournalValue>(
                new ConcurrentLinkedNodesCollection<JournalRecord<TJournalValue>>(),
                new SkippingLinkedNodesCollectionCleanerDecorator<JournalRecord<TJournalValue>>(
                    new TailCutLinkedNodesCollectionCleaner<JournalRecord<TJournalValue>>(
                        new TimeSpanTailDetectPolicy<TJournalValue>(TimeSpan.FromSeconds(5), _dateTimeManager)), 10));
        }
    }
}
