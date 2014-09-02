using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Journals;

namespace Saut.StateModel.Test.Journals
{
    [TestFixture]
    public class TimeSpanTailDetectPolicyTests
    {
        [Test]
        public void TestTailDetection()
        {
            DateTime t0 = DateTime.Today;
            var ts = TimeSpan.FromSeconds(10);
            var dtm = MockRepository.GenerateMock<IDateTimeManager>();
            dtm.Stub(m => m.Now).Return(t0);
            var detector = new TimeSpanTailDetectPolicy<int>(ts, dtm);
            var collection = Enumerable.Range(0, 20).Select(i => new ConcurrentLogNode<JournalRecord<int>>(new JournalRecord<int>(t0.AddSeconds(-i), i))).ToList();
            var tailElement = detector.GetLastActualElement(collection);
            Assert.AreEqual(t0 - ts, tailElement.Item.Time);
        }
    }
}