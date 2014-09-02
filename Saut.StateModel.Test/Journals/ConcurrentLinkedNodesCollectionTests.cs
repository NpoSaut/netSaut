using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Saut.StateModel.Journals;

namespace Saut.StateModel.Test.Journals
{
    [TestFixture]
    public class ConcurrentLinkedNodesCollectionTests
    {
        [Test, Description("Проверяет добавление и извлечения одного (головного) элемента")]
        public void AddFirstRecordTest()
        {
            var collection = new ConcurrentLinkedNodesCollection<int>();
            collection.TryInsert(5, null);
            List<ConcurrentLogNode<int>> output = collection.ToList();
            Assert.AreEqual(1, output.Count, "Количество элементов в коллекции после добавления одной записи не соответствует ожидаемому");
            Assert.AreEqual(5, output.First().Item, "Из коллекции было извлечено не то значение, которое было положено в неё");
        }

        [Test, Description("Добавляет в коллекцию несколько записей, каждый раз добавляя запись в голову коллекции")]
        public void CollectionInsertAtHeadTest()
        {
            DateTime t0 = DateTime.Today;
            int[] input = Enumerable.Range(0, 10).Select(i => i * 10).ToArray();
            var collection = new ConcurrentLinkedNodesCollection<int>();
            foreach (int i in input)
                Assert.AreEqual(true, collection.TryInsert(i, null), "Неудачная попытка добавить элемент в голову коллекции");
            List<int> output = collection.Select(n => n.Item).ToList();
            Assert.AreEqual(input.Length, output.Count, "Вероятно, были потеряны какие-то элементы");
            CollectionAssert.AreEquivalent(input.Reverse(), output, "Список элементов после извлечения из коллекции не соответствует входному списку");
        }

        [Test, Description("Добавляет в коллекцию несколько записей, каждый раз добавляя запись в голову хвост")]
        public void CollectionInsertToTailTest()
        {
            DateTime t0 = DateTime.Today;
            int[] input = Enumerable.Range(0, 10).Select(i => i * 10).ToArray();
            var collection = new ConcurrentLinkedNodesCollection<int>();
            foreach (int i in input)
                Assert.AreEqual(true, collection.TryInsert(i, collection.LastOrDefault()), "Неудачная попытка добавить элемент в хвост коллекции");
            List<int> output = collection.Select(n => n.Item).ToList();
            Assert.AreEqual(input.Length, output.Count, "Вероятно, были потеряны какие-то элементы");
            CollectionAssert.AreEquivalent(input, output, "Список элементов после извлечения из коллекции не соответствует входному списку");
        }

        [Test, Description("Бычий тест на потоки -- добавление")]
        public void ThreadAddStressTest()
        {
            const int threadsCount = 6;
            const int recordsCount = 30;
            List<List<int>> originRecords =
                Enumerable.Range(0, threadsCount)
                          .Select(t =>
                                  Enumerable.Range(0, recordsCount)
                                            .Select(i => i * threadsCount + t)
                                            .ToList())
                          .ToList();

            var collection = new ConcurrentLinkedNodesCollection<int>();

            List<Task> tasks = originRecords.Select(recordsGroup =>
                                                    new Task(() =>
                                                             {
                                                                 foreach (int i in recordsGroup)
                                                                 {
                                                                     bool success;
                                                                     do
                                                                     {
                                                                         success = collection.TryInsert(i, collection.FirstOrDefault(n => n.Item > i));
                                                                     } while (!success);
                                                                 }
                                                             }))
                                            .ToList();
            foreach (Task task in tasks)
                task.Start();

            SpinWait.SpinUntil(() => tasks.All(t => t.IsCompleted), TimeSpan.FromSeconds(1));

            List<int> expectedList = originRecords.SelectMany(rg => rg).OrderBy(i => i).ToList();
            List<int> extractedRecords = collection.Select(n => n.Item).ToList();
            Assert.AreEqual(expectedList.Count, extractedRecords.Count, "В ходе многопоточного помещения элементов в журнал какие-то элементы были потеряны");
            CollectionAssert.AreEquivalent(expectedList, extractedRecords, "Коллекция элементов исказилась после многопоточного помещения в журнал");
        }
    }
}
