using Geographics;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.StateProperties
{
    /// <summary>GPS-координата</summary>
    public class GpsPositionProperty : InterpolatablePropertyBase<EarthPoint>
    {
        public GpsPositionProperty(IDateTimeManager TimeManager, IJournal<EarthPoint> Journal, IInterpolator<EarthPoint> Interpolator,
                                   IRecordPicker RecordPicker) : base(TimeManager, Journal, Interpolator, RecordPicker) { }

        /// <summary>Название свойства.</summary>
        public override string Name
        {
            get { return "GPS-координата"; }
        }
    }
}
