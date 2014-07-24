using BlokFrames;

namespace Saut.FrameProcessing
{
    public interface IFrameProcessor { }

    public interface IFrameProcessor<TFrame> : IFrameProcessor where TFrame : BlokFrame
    {
        void ProcessFrame(TFrame Frame);
    }
}
