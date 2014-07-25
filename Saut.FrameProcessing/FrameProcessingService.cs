using System;
using System.Linq;
using BlokFrames;

namespace Saut.FrameProcessing
{
    /// <summary>Сервис обработки входящих сообщений.</summary>
    public class FrameProcessingService : IFrameProcessingService
    {
        private readonly ILookup<Type, IFrameProcessor> _frameProcessors;

        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        public FrameProcessingService(IFrameProcessor[] FrameProcessors) { _frameProcessors = FrameProcessors.ToLookup(p => p.FrameType); }

        public void ProcessFrame(BlokFrame Frame)
        {
            foreach (IFrameProcessor processor in _frameProcessors[Frame.GetType()])
                processor.ProcessFrame(Frame);
        }
    }
}
