
using BlokFrames;
using Communications.Can;

namespace Saut.Communication.Interfaces
{
    public interface IMessagesDecoder
    {
        BlokFrame DecodeFrame(CanFrame Frame);
    }
}