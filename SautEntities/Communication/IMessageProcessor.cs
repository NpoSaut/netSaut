using System;
using BlokFrames;

namespace Saut.Communication
{
    /// <summary>Обработчик поступающих сообщений</summary>
    public interface IMessageProcessor
    {
        /// <summary>Тип обрабатываемых сообщений</summary>
        Type MessageType { get; }

        /// <summary>Обрабатывает поступившее сообщение</summary>
        /// <param name="Message">Сообщение</param>
        void ProcessMessage(BlokFrame Message);
    }
}
