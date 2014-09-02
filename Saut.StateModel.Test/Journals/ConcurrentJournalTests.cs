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
        [Test, Description("Проверяет, вызывает ли журнал Cleaner-а после каждого добавления записи")]
        public void CallsCleanerTest()
        {
            var record = new JournalRecord<int>(DateTime.Today, 5);

            var collectionMock = MockRepository.GenerateMock<IConcurrentLinkedCollection<JournalRecord<int>>>();
            collectionMock.Stub(c => c.GetEnumerator()).Return(Enumerable.Empty<ConcurrentLogNode<JournalRecord<int>>>().GetEnumerator());
            int insertionAttemptsCounter = 0;
            collectionMock.Stub(c => c.TryInsert(record, null)).Return(false).WhenCalled(Invocation => Invocation.ReturnValue = ++insertionAttemptsCounter == 3);

            var cleanerMock = MockRepository.GenerateMock<ILinkedNodesCollectionCleaner<JournalRecord<int>>>();
            cleanerMock.Expect(c => c.Cleanup(collectionMock));

            var journal = new ConcurrentJournal<int>(collectionMock, cleanerMock);
            journal.AddRecord(record);

            Assert.AreEqual(3, insertionAttemptsCounter, "Журнал не предпринял достаточное количество попыток добавления элемента");
            cleanerMock.VerifyAllExpectations();
            collectionMock.VerifyAllExpectations();
        }

        [Test, Description("Проверяет, вызывает ли журнал Cleaner-а после каждого добавления записи")]
        public void InsertItemAtTailTest()
        {
            var record0 = new JournalRecord<int>(DateTime.Today.AddSeconds(1), 8);
            var record1 = new JournalRecord<int>(DateTime.Today, 5);

            var nodes = new System.Collections.Generic.List<ConcurrentLogNode<JournalRecord<int>>> { new ConcurrentLogNode<JournalRecord<int>>(record0) };

            var collectionMock = MockRepository.GenerateMock<IConcurrentLinkedCollection<JournalRecord<int>>>();
            collectionMock.Stub(c => c.GetEnumerator()).Return(nodes.GetEnumerator());
            collectionMock.Stub(c => c.TryInsert(record1, nodes[0])).Return(true);

            var cleanerMock = MockRepository.GenerateMock<ILinkedNodesCollectionCleaner<JournalRecord<int>>>();
            cleanerMock.Expect(c => c.Cleanup(collectionMock));

            var journal = new ConcurrentJournal<int>(collectionMock, cleanerMock);
            journal.AddRecord(record1);
            cleanerMock.VerifyAllExpectations();
            collectionMock.VerifyAllExpectations();
        }
    }
}
