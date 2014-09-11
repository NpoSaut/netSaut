using Geographics;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Obsoleting;

namespace Saut.StateModel.StateProperties
{
    /// <summary>GPS-координата</summary>
    public class GpsPositionProperty : InterpolatablePropertyBase<EarthPoint>
    {
        public GpsPositionProperty(IDateTimeManager TimeManager, IJournalFactory<EarthPoint> JournalFactory, IInterpolator<EarthPoint> Interpolator,
                                   IRecordPicker RecordPicker, IObsoletePolicyProvider ObsoletePolicyProvider)
            : base(TimeManager, JournalFactory, Interpolator, RecordPicker, ObsoletePolicyProvider) { }

        /// <summary>Название свойства.</summary>
        public override string Name
        {
            get { return "GPS-координата"; }
        }
    }
}
