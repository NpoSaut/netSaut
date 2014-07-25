using System;
using BlokFrames;

namespace Saut.FrameProcessing
{
    /// <summary>Базовая шаблонная реализация интерфейса <see cref="IFrameProcessor" /> .</summary>
    /// <remarks>Этот базовый класс делает более удобной реализацию метода ProcessFrame, заменяя параметр общего класса
    ///     <see cref="BlokFrame" /> на
    ///     <typeparamref name="TFrame" /> и реализуя свойство FrameType.</remarks>
    /// <typeparam name="TFrame">Тип обрабатываемого сообщения.</typeparam>
    public abstract class FrameProcessorBase<TFrame> : IFrameProcessor
        where TFrame : BlokFrame
    {
        public Type FrameType
        {
            get { return typeof (TFrame); }
        }

        public void ProcessFrame(BlokFrame Frame) { ImplementProcessFrame((TFrame)Frame); }

        /// <summary>Реализация процедуры обработки сообщения.</summary>
        /// <param name="Frame">Принятое сообщение.</param>
        protected abstract void ImplementProcessFrame(TFrame Frame);
    }
}
