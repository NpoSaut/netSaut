using BlokFrames;
using Geographics;
using Saut.StateModel.StateProperties;

namespace Saut.Communication.BlokFrameProcessors
{
    /// <summary>Обработчик сообщения <seealso cref="MmAltLongFrame"/> с GPS-данными</summary>
    public class MmAltLongProcessor : MessageProcessorBase<MmAltLongFrame>
    {
        private readonly GpsPositionProperty _positionProperty;
        private readonly GpsReliabilityProperty _reliabilityProperty;

        public MmAltLongProcessor(GpsPositionProperty PositionProperty, GpsReliabilityProperty ReliabilityProperty)
        {
            _positionProperty = PositionProperty;
            _reliabilityProperty = ReliabilityProperty;
        }

        /// <summary>Обрабатывает поступившее сообщение</summary>
        /// <param name="Message">Сообщение</param>
        public override void ProcessMessage(MmAltLongFrame Message)
        {
            var position = new EarthPoint(Message.Latitude, Message.Longitude);
            _positionProperty.UpdateValue(position, Message.Time);
            _reliabilityProperty.UpdateValue(Message.Reliable, Message.Time);
        }
    }
}
