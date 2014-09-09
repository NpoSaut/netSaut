using System;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.StateProperties
{
    /// <summary>Скорость</summary>
    public class SpeedProperty : InterpolatablePropertyBase<Double>
    {
        public SpeedProperty(IDateTimeManager TimeManager, IJournalFactory<double> JournalFactory, IInterpolator<double> Interpolator,
                             IRecordPicker RecordPicker) : base(TimeManager, JournalFactory, Interpolator, RecordPicker) { }

        /// <summary>Название свойства.</summary>
        public override string Name
        {
            get { return "Скорость"; }
        }
    }
}
