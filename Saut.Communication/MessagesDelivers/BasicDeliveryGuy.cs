using System;
using System.Linq;
using BlokFrames;
using Saut.Communication.Interfaces;

namespace Saut.Communication.ProcessingServices
{
    /// <summary>Простой сервис обработки сообщений</summary>
    /// <remarks>
    ///     В момент создания принимает массив обработчиков сообщений и передаёт каждое полученное сообщение все
    ///     соответствующим обработчикам
    /// </remarks>
    public class BasicDeliveryGuy : IDeliveryGuy
    {
        private readonly IMessageProcessor[] _processors;
        public BasicDeliveryGuy(IMessageProcessor[] Processors) { _processors = Processors; }

        /// <summary>Доставляет сообщение всем обработчикам</summary>
        /// <param name="Message">Свеже полученное сообщение</param>
        public void DeliverMessage(BlokFrame Message)
        {
            Type messageType = Message.GetType();
            foreach (IMessageProcessor messageProcessor 
                in _processors.Where(p => p.MessageType.IsAssignableFrom(messageType)))
                messageProcessor.ProcessMessage(Message);
        }
    }
}
