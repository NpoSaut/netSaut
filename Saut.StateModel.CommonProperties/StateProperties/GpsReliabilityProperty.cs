using System;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.StateProperties
{
    /// <summary>Достоверность сигнала GPS</summary>
    public class GpsReliabilityProperty : InterpolatablePropertyBase<Boolean>
    {
        public GpsReliabilityProperty(IDateTimeManager TimeManager, IJournalFactory<bool> JournalFactory, IInterpolator<bool> Interpolator,
                                      IRecordPicker RecordPicker) : base(TimeManager, JournalFactory, Interpolator, RecordPicker) { }

        /// <summary>Название свойства.</summary>
        public override string Name
        {
            get { return "Достоверность сигнала GPS"; }
        }
    }
}
