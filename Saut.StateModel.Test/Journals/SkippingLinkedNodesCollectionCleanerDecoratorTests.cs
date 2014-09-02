using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Journals;

namespace Saut.StateModel.Test.Journals
{
    [TestFixture]
    public class SkippingLinkedNodesCollectionCleanerDecoratorTests
    {
        [Test, Description("Проверяет корректность работы при запуске из нескольких потоков")]
        public void MultiThreadedTest()
        {
            const int threadsCount = 6;
            const int callsPerThread = 15;
            const int skipCycle = 5;

            var collectionMock = MockRepository.GenerateMock<IEnumerable<ConcurrentLogNode<int>>>();

            var baseCleaner = MockRepository.GenerateMock<ILinkedNodesCollectionCleaner<int>>();
            baseCleaner.Expect(b => b.Cleanup(collectionMock))
                       .Message("Ожидается, что метод базового класса будет вызван указанное количество раз")
                       .Repeat.Times(callsPerThread * threadsCount / skipCycle);

            var decorator = new SkippingLinkedNodesCollectionCleanerDecorator<int>(baseCleaner, skipCycle);

            List<Task> tasks =
                Enumerable.Range(0, threadsCount)
                          .Select(i => new Task(() =>
                                                {
                                                    for (int j = 0; j < callsPerThread; j++)
                                                        decorator.Cleanup(collectionMock);
                                                }))
                          .ToList();

            foreach (Task task in tasks) task.Start();
            SpinWait.SpinUntil(() => tasks.All(t => t.IsCompleted));

            baseCleaner.VerifyAllExpectations();
        }

        [Test, Description("Проверяет корректность работы при запуске из одного потока")]
        public void SingleThreadedTest()
        {
            const int calls = 15;
            const int skipCycle = 5;

            var collectionMock = MockRepository.GenerateMock<IEnumerable<ConcurrentLogNode<int>>>();

            var baseCleaner = MockRepository.GenerateMock<ILinkedNodesCollectionCleaner<int>>();
            baseCleaner.Expect(b => b.Cleanup(collectionMock))
                       .Message("Ожидается, что метод базового класса будет вызван указанное количество раз")
                       .Repeat.Times(calls / skipCycle);

            var decorator = new SkippingLinkedNodesCollectionCleanerDecorator<int>(baseCleaner, skipCycle);
            for (int i = 0; i < 15; i++)
                decorator.Cleanup(collectionMock);

            baseCleaner.VerifyAllExpectations();
        }
    }
}
