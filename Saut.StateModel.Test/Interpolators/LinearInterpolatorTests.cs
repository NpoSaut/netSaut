using System;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Exceptions;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Interpolators;
using Saut.StateModel.Interpolators.InterpolationTools;

namespace Saut.StateModel.Test.Interpolators
{
    [TestFixture]
    public class LinearInterpolatorTests
    {
        [Test]
        public void SimpleTest()
        {
            DateTime t0 = DateTime.Today;
            var pick = MockRepository.GenerateMock<IJournalPick<Double>>();
            pick.Stub(p => p.RecordsAfter).Return(new[] { new JournalRecord<double>(t0.AddMilliseconds(150), 3000) });
            pick.Stub(p => p.RecordsBefore).Return(new[] { new JournalRecord<double>(t0.AddMilliseconds(50), 1000) });

            var wt = MockRepository.GenerateMock<IWeightingTool<double>>();
            wt.Expect(t => t.GetWeightedArithmeticMean(1000, 3000, 0.0)).Return(1000);
            wt.Expect(t => t.GetWeightedArithmeticMean(1000, 3000, 0.5)).Return(2000);
            wt.Expect(t => t.GetWeightedArithmeticMean(1000, 3000, 1.0)).Return(3000);

            var interpolator = new LinearInterpolator<Double>(wt);

            Assert.AreEqual(interpolator.Interpolate(pick, t0.AddMilliseconds(50)), 1000);
            Assert.AreEqual(interpolator.Interpolate(pick, t0.AddMilliseconds(100)), 2000);
            Assert.AreEqual(interpolator.Interpolate(pick, t0.AddMilliseconds(150)), 3000);
            wt.VerifyAllExpectations();
        }

        [Test, Description("Проверяет, будет ли вызываться исключение PropertyValueUndefinedException при недостаточном количестве элементов в JournalPick")]
        [ExpectedException(typeof (PropertyValueUndefinedException))]
        public void TestPropertyValueUndefinedException()
        {
            var pick = MockRepository.GenerateMock<IJournalPick<Double>>();
            pick.Stub(p => p.RecordsAfter).Return(new JournalRecord<double>[] { });
            pick.Stub(p => p.RecordsBefore).Return(new JournalRecord<double>[] { });

            var wt = MockRepository.GenerateMock<IWeightingTool<double>>();
            var interpolator = new LinearInterpolator<Double>(wt);

            interpolator.Interpolate(pick, DateTime.Today);
        }
    }
}
