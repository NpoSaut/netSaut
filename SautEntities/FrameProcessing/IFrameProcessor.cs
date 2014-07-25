using System;
using BlokFrames;

namespace Saut.FrameProcessing
{
    /// <summary>Обработчик входящих сообщений</summary>
    public interface IFrameProcessor
    {
        /// <summary>Тип обрабатываемых сообщений</summary>
        Type FrameType { get; }

        /// <summary>Обрабатывает указанное входящее сообщение</summary>
        /// <param name="Frame">Входящее сообщение</param>
        void ProcessFrame(BlokFrame Frame);
    }
}
