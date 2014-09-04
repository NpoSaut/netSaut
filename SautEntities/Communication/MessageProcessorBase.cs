using System;
using BlokFrames;

namespace Saut.Communication
{
    /// <summary>Базовый класс для generic-обработчиков сообщений</summary>
    /// <typeparam name="TMessage">Тип обрабатываемого сообщения</typeparam>
    public abstract class MessageProcessorBase<TMessage> : IMessageProcessor where TMessage : BlokFrame
    {
        /// <summary>Тип обрабатываемых сообщений</summary>
        public Type MessageType
        {
            get { return typeof (TMessage); }
        }

        /// <summary>Обрабатывает поступившее сообщение</summary>
        /// <param name="Message">Сообщение</param>
        public void ProcessMessage(BlokFrame Message) { ProcessMessage((TMessage)Message); }

        /// <summary>Обрабатывает поступившее сообщение</summary>
        /// <param name="Message">Сообщение</param>
        public abstract void ProcessMessage(TMessage Message);
    }
}
