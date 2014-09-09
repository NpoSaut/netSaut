using Geographics;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.StateProperties
{
    /// <summary>GPS-координата</summary>
    public class GpsPositionProperty : InterpolatablePropertyBase<EarthPoint>
    {
        public GpsPositionProperty(IDateTimeManager TimeManager, IJournalFactory<EarthPoint> JournalFactory, IInterpolator<EarthPoint> Interpolator,
                                   IRecordPicker RecordPicker) : base(TimeManager, JournalFactory, Interpolator, RecordPicker) { }

        /// <summary>Название свойства.</summary>
        public override string Name
        {
            get { return "GPS-координата"; }
        }
    }
}
