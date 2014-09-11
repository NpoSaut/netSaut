using System;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Exceptions;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Interpolators;

namespace Saut.StateModel.Test.Interpolation
{
    [TestFixture]
    public class StepInterpolatorTests
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

        [Test, Description("Проверяет, будет ли вызываться исключение PropertyValueUndefinedException при отсутствии элементов в Pick.Before")]
        [ExpectedException(typeof(PropertyValueUndefinedException))]
        public void TestPropertyValueUndefinedException()
        {
            var t0 = DateTime.Today;
            var pick = MockRepository.GenerateMock<IJournalPick<String>>();
            pick.Stub(p => p.RecordsBefore).Return(new JournalRecord<String>[0]);
            var interpolator = new StepInterpolator<String>();
            interpolator.Interpolate(pick, t0);
        }
    }
}