using System;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Interpolators;

namespace Saut.StateModel.Test.Interpolators
{
    [TestFixture]
    public class LinearInterpolatorTest
    {
        [Test]
        public void SimpleTest()
        {
            var t0 = DateTime.Today;
            var pick = MockRepository.GenerateMock<IJournalPick<Double>>();
            pick.Stub(p => p.RecordsAfter).Return(new[] { new JournalRecord<double>(t0.AddMilliseconds(150), 3000) });
            pick.Stub(p => p.RecordsBefore).Return(new[] { new JournalRecord<double>(t0.AddMilliseconds(50), 1000) });
            var interpolator = new LinearInterpolator();
            Assert.AreEqual(interpolator.Interpolate(pick, t0.AddMilliseconds(50)), 1000);
            Assert.AreEqual(interpolator.Interpolate(pick, t0.AddMilliseconds(100)), 2000);
            Assert.AreEqual(interpolator.Interpolate(pick, t0.AddMilliseconds(150)), 3000);
        }

        [Test]
        public void FutureTest()
        {
            var t0 = DateTime.Today;
            var pick = MockRepository.GenerateMock<IJournalPick<Double>>();
            pick.Stub(p => p.RecordsAfter).Return(new JournalRecord<double>[0]);
            pick.Stub(p => p.RecordsBefore).Return(new[] { new JournalRecord<double>(t0.AddMilliseconds(50), 1000), new JournalRecord<double>(t0.AddMilliseconds(0), 0) });
            var interpolator = new LinearInterpolator();
            Assert.AreEqual(interpolator.Interpolate(pick, t0.AddMilliseconds(50)), 1000);
            Assert.AreEqual(interpolator.Interpolate(pick, t0.AddMilliseconds(100)), 2000);
            Assert.AreEqual(interpolator.Interpolate(pick, t0.AddMilliseconds(150)), 3000);
        }
    }
}