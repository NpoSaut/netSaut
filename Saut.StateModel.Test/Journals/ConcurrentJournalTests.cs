using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Journals;

namespace Saut.StateModel.Test.Journals
{
    [TestFixture]
    public class ConcurrentJournalTests
    {
        [Test, Description("Проверяет добавление и извлечения одного (головного) элемента")]
        public void AddFirstRecordTest()
        {
            DateTime dt = DateTime.Today;
            var journal = new ConcurrentJournal<int>(MockRepository.GenerateMock<ILinkedNodesCollectionCleaner<int>>());
            journal.AddRecord(5, dt);
            List<JournalRecord<int>> records = journal.Records.ToList();
            Assert.AreEqual(1, records.Count, "Количество элементов в коллекции после добавления одной записи не соответствует ожидаемому");
            JournalRecord<int> rec = records.First();
            Assert.AreEqual(5, rec.Value, "Нарушилось значение записи");
            Assert.AreEqual(dt, rec.Time, "Нарушилось время записи");
        }

        [Test, Description("Проверяет, вызывает ли журнал Cleaner-а после каждого добавления записи")]
        public void CallsCleanerTest()
        {
            var cleanerMock = MockRepository.GenerateMock<ILinkedNodesCollectionCleaner<int>>();
            cleanerMock.Expect(c => c.Cleanup(null)).IgnoreArguments();
            var journal = new ConcurrentJournal<int>(cleanerMock);
            journal.AddRecord(5, default(DateTime));
            cleanerMock.VerifyAllExpectations();
        }

        [Test, Description("Добавляет и извлекает коллекцию записей, проверяет её на то, чтобы она не исказилась")]
        public void CollectionAddAndExtractTest()
        {
            DateTime t0 = DateTime.Today;
            var originRecords =
                new[]
                {
                    new JournalRecord<int>(t0.AddHours(1), 1),
                    new JournalRecord<int>(t0.AddHours(2), 2),
                    new JournalRecord<int>(t0.AddHours(3), 3),
                    new JournalRecord<int>(t0.AddHours(4), 4),
                    new JournalRecord<int>(t0.AddHours(5), 5)
                };
            var journal = new ConcurrentJournal<int>(MockRepository.GenerateMock<ILinkedNodesCollectionCleaner<int>>());
            foreach (var record in originRecords)
                journal.AddRecord(record);
            List<JournalRecord<int>> extractedRecords = journal.Records.ToList();
            CollectionAssert.AreEquivalent(originRecords, extractedRecords, "Коллекция элементов исказилась после помещения в журнал");
        }

        [Test, Description("Бычий тест на потоки -- добавление")]
        public void ThreadAddStressTest()
        {
            DateTime t0 = DateTime.Today;
            const int threadsCount = 6;
            const int recordsCount = 30;
            List<List<JournalRecord<int>>> originRecords =
                Enumerable.Range(0, threadsCount)
                          .Select(t =>
                                  Enumerable.Range(0, recordsCount)
                                            .Select(i => new JournalRecord<int>(t0.AddHours(i).AddMinutes(10 * t), i * threadsCount + t))
                                            .ToList())
                          .ToList();

            var journal = new ConcurrentJournal<int>(MockRepository.GenerateMock<ILinkedNodesCollectionCleaner<int>>());

            List<Task> tasks = originRecords.Select(recordsGroup =>
                                                    new Task(() =>
                                                             {
                                                                 foreach (var record in recordsGroup)
                                                                     journal.AddRecord(record);
                                                             }))
                                            .ToList();
            foreach (Task task in tasks)
                task.Start();

            SpinWait.SpinUntil(() => tasks.All(t => t.IsCompleted), TimeSpan.FromSeconds(1));

            List<JournalRecord<int>> expectedList = originRecords.SelectMany(rg => rg).OrderBy(r => r.Time).ToList();
            List<JournalRecord<int>> extractedRecords = journal.Records.ToList();
            Assert.AreEqual(expectedList.Count, extractedRecords.Count, "В ходе многопоточного помещения элементов в журнал какие-то элементы были потеряны");
            CollectionAssert.AreEquivalent(expectedList, extractedRecords, "Коллекция элементов исказилась после многопоточного помещения в журнал");
        }
    }
}
