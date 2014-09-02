using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Journals;

namespace Saut.StateModel.Test.Journals
{
    [TestFixture]
    public class TailCutLinkedNodesCollectionCleanerTests
    {
        private static TailCutLinkedNodesCollectionCleaner<int> GetConfiguredCutter(DateTime nowTime, TimeSpan actualityTimeSpan)
        {
            var dtm = MockRepository.GenerateMock<IDateTimeManager>();
            dtm.Stub(m => m.Now).Return(nowTime);
            return new TailCutLinkedNodesCollectionCleaner<int>(actualityTimeSpan, dtm);
        }

//        [Test, Description("Проверяет, обрезает ли он всё после указанной даты")]
//        public void TestCut()
//        {
//            var t0 = DateTime.Today;
//            var cutter = GetConfiguredCutter(t0, TimeSpan.FromSeconds(10));
//            var originalCollection = Enumerable.Range(0, 20).Select(i => new ConcurrentLogNode<int>(new JournalRecord<int>(t0.AddSeconds(i), i)));
//            var collection = Enumerable.Range(0, 20).Select(i => new ConcurrentLogNode<int>(new JournalRecord<int>(t0.AddSeconds(i), i)));
//            cutter.Cleanup(collection);
//            CollectionAssert.AreEquivalent();
//        }
    }
}
