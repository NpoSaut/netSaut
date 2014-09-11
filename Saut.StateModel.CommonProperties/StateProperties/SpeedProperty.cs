using System;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Obsoleting;

namespace Saut.StateModel.StateProperties
{
    /// <summary>Скорость</summary>
    public class SpeedProperty : InterpolatablePropertyBase<Double>
    {
        public SpeedProperty(IDateTimeManager TimeManager, IJournalFactory<double> JournalFactory, IInterpolator<double> Interpolator,
                             IRecordPicker RecordPicker, IObsoletePolicyProvider ObsoletePolicyProvider)
            : base(TimeManager, JournalFactory, Interpolator, RecordPicker, ObsoletePolicyProvider) { }

        /// <summary>Название свойства.</summary>
        public override string Name
        {
            get { return "Скорость"; }
        }
    }
}
