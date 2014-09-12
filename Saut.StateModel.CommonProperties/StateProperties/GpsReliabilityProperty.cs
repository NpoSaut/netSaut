using System;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Obsoleting;

namespace Saut.StateModel.StateProperties
{
    /// <summary>Достоверность сигнала GPS</summary>
    public class GpsReliabilityProperty : InterpolatablePropertyBase<Boolean>
    {
        public GpsReliabilityProperty(IDateTimeManager TimeManager, IJournalFactory<bool> JournalFactory, IInterpolator<bool> Interpolator,
                                      IRecordPicker RecordPicker, IObsoletePolicyProvider ObsoletePolicyProvider)
            : base(TimeManager, JournalFactory, Interpolator, RecordPicker, ObsoletePolicyProvider) { }

        /// <summary>Название свойства.</summary>
        public override string Name
        {
            get { return "Достоверность сигнала GPS"; }
        }
    }
}
