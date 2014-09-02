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
    public class IntegrationTests
    {
        [Test, Description("Бычий тест на потоки -- добавление")]
        public void BullyThreadAddStressTest()
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

            var journal = new ConcurrentJournal<int>(
                new ConcurrentLinkedNodesCollection<JournalRecord<int>>(), 
                MockRepository.GenerateMock<ILinkedNodesCollectionCleaner<JournalRecord<int>>>());

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