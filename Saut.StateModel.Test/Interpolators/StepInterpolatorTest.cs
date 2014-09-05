using System;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Interpolators;

namespace Saut.StateModel.Test.Interpolation
{
    [TestFixture]
    public class StepInterpolatorTest
    {
        [Test]
        public void StepTest()
        {
            var t0 = DateTime.Today;
            var pick = MockRepository.GenerateMock<IJournalPick<String>>();
            pick.Stub(p => p.RecordsBefore).Return(new[] { new JournalRecord<String>(t0.AddMilliseconds(50), "abc") });
            var interpolator = new StepInterpolator<String>();
            Assert.AreEqual(interpolator.Interpolate(pick, t0.AddMilliseconds(50)), "abc", "Значение в начале ступени не соответствует ожидаемому");
            Assert.AreEqual(interpolator.Interpolate(pick, t0.AddMilliseconds(100)), "abc", "Значение в середине ступени не соответствует ожидаемому");
        }
    }
}