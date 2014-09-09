using BlokFrames;
using Saut.StateModel.StateProperties;

namespace Saut.Communication.BlokFrameProcessors
{
    /// <summary>Обработчик сообщений <seealso cref="IpdState" /> от БС-ДПС</summary>
    public class IpdStateProcessor : MessageProcessorBase<IpdState>
    {
        private readonly SpeedProperty _speedProperty;
        public IpdStateProcessor(SpeedProperty SpeedProperty) { _speedProperty = SpeedProperty; }

        /// <summary>Обрабатывает поступившее сообщение</summary>
        /// <param name="Message">Сообщение</param>
        public override void ProcessMessage(IpdState Message) { _speedProperty.UpdateValue(Message.Speed, Message.Time); }
    }
}
