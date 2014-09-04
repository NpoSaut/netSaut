using System;
using System.Linq;
using BlokFrames;
using Saut.Communication.Interfaces;

namespace Saut.Communication.ProcessingServices
{
    /// <summary>Сервис обработки входящих сообщений.</summary>
    public class LookupDeliveryGuy : IDeliveryGuy
    {
        private readonly ILookup<Type, IMessageProcessor> _frameProcessors;

        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        public LookupDeliveryGuy(IMessageProcessor[] FrameProcessors) { _frameProcessors = FrameProcessors.ToLookup(p => p.MessageType); }

        /// <summary>Доставляет сообщение всем обработчикам</summary>
        /// <param name="Message">Свеже полученное сообщение</param>
        public void DeliverMessage(BlokFrame Message)
        {
            foreach (IMessageProcessor processor in _frameProcessors[Message.GetType()])
                processor.ProcessMessage(Message);
        }
    }
}
