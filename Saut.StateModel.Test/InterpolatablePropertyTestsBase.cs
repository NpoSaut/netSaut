using System;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Test
{
    public abstract class InterpolatablePropertyTestsBase<TValue>
    {
        protected abstract TValue TestValue { get; }

        protected abstract InterpolatablePropertyBase<TValue> GetInstance(IDateTimeManager TimeManager, IJournalFactory<TValue> JournalFactory, IInterpolator<TValue> Interpolator, IRecordPicker RecordPicker);

        private InterpolatablePropertyBase<TValue> GetInstance(TestSuit ts) { return GetInstance(ts.TimeManager, ts.JournalFactory, ts.Interpolator, ts.Picker); }

        [Test]
        public void TestUpdateCurrentValue()
        {
            var ts = new TestSuit();
            ts.JournalMock.Expect(j => j.AddRecord(TestValue, ts.TimeManager.Now));

            InterpolatablePropertyBase<TValue> property = GetInstance(ts);
            property.UpdateValue(TestValue);

            ts.JournalMock.VerifyAllExpectations();
        }

        [Test]
        public void TestUpdateCustomValue()
        {
            var ts = new TestSuit();

            DateTime probeTime = ts.TimeManager.Now.AddSeconds(-15);

            ts.JournalMock.Expect(j => j.AddRecord(TestValue, probeTime));

            InterpolatablePropertyBase<TValue> property = GetInstance(ts);
            property.UpdateValue(TestValue, probeTime);

            ts.JournalFactory.VerifyAllExpectations();
        }

        [Test]
        public void TestGetValue()
        {
            var ts = new TestSuit();

            DateTime probeTime = ts.TimeManager.Now.AddSeconds(-15);

            var pick = MockRepository.GenerateMock<IJournalPick<TValue>>();
            ts.Picker
              .Expect(p => p.PickRecords(ts.JournalMock, probeTime))
              .Return(pick);
            ts.Interpolator
              .Expect(i => i.Interpolate(pick, probeTime))
              .Return(TestValue);

            InterpolatablePropertyBase<TValue> property = GetInstance(ts);
            TValue val = property.GetValue(probeTime);

            ts.Picker.VerifyAllExpectations();
            ts.Interpolator.VerifyAllExpectations();
            Assert.AreEqual(TestValue, val);
        }

        protected class TestSuit
        {
            public TestSuit() : this(new DateTime(2000, 01, 01)) { }

            public TestSuit(DateTime t0)
            {
                TimeManager = MockRepository.GenerateMock<IDateTimeManager>();
                JournalMock = MockRepository.GenerateMock<IJournal<TValue>>();
                JournalFactory = MockRepository.GenerateMock<IJournalFactory<TValue>>();
                Interpolator = MockRepository.GenerateMock<IInterpolator<TValue>>();
                Picker = MockRepository.GenerateMock<IRecordPicker>();

                TimeManager.Stub(m => m.Now).Return(t0);
                JournalFactory.Stub(jf => jf.GetJournal()).Return(JournalMock);
            }

            public IJournal<TValue> JournalMock { get; private set; }
            public IDateTimeManager TimeManager { get; private set; }
            public IJournalFactory<TValue> JournalFactory { get; private set; }
            public IInterpolator<TValue> Interpolator { get; private set; }
            public IRecordPicker Picker { get; private set; }
        }
    }

    [TestFixture]
    public class InterpolatablePropertyBaseTests : InterpolatablePropertyTestsBase<int>
    {
        protected override int TestValue
        {
            get { return 17; }
        }

        protected override InterpolatablePropertyBase<int> GetInstance(IDateTimeManager TimeManager, IJournalFactory<int> JournalFactory, IInterpolator<int> Interpolator, IRecordPicker RecordPicker)
        {
            return new MyProperty(TimeManager, JournalFactory, Interpolator, RecordPicker);
        }

        private class MyProperty : InterpolatablePropertyBase<int>
        {
            public MyProperty(IDateTimeManager TimeManager, IJournalFactory<int> JournalFactory, IInterpolator<int> Interpolator, IRecordPicker RecordPicker)
                : base(TimeManager, JournalFactory, Interpolator, RecordPicker) { }

            /// <summary>Название свойства.</summary>
            public override string Name
            {
                get { return "My Test Property"; }
            }
        }
    }
}
