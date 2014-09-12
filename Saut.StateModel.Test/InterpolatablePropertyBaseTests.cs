using NUnit.Framework;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Obsoleting;

namespace Saut.StateModel.Test
{
    [TestFixture]
    public class InterpolatablePropertyBaseTests : InterpolatablePropertyTestsBase<int>
    {
        protected override int TestValue
        {
            get { return 17; }
        }

        protected override InterpolatablePropertyBase<int> GetInstance(IDateTimeManager TimeManager, IJournalFactory<int> JournalFactory,
                                                                       IInterpolator<int> Interpolator, IRecordPicker RecordPicker,
                                                                       IObsoletePolicyProvider ObsoletePolicyProvider)
        {
            return new MyProperty(TimeManager, JournalFactory, Interpolator, RecordPicker, ObsoletePolicyProvider);
        }

        private class MyProperty : InterpolatablePropertyBase<int>
        {
            public MyProperty(IDateTimeManager TimeManager, IJournalFactory<int> JournalFactory, IInterpolator<int> Interpolator, IRecordPicker RecordPicker,
                              IObsoletePolicyProvider ObsoletePolicyProvider)
                : base(TimeManager, JournalFactory, Interpolator, RecordPicker, ObsoletePolicyProvider) { }

            /// <summary>Название свойства.</summary>
            public override string Name
            {
                get { return "My Test Property"; }
            }
        }
    }
}
