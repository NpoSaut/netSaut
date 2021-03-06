﻿using BlokFrames;
using Communications.Can;
using Saut.Communication.Interfaces;

namespace Saut.Communication.Decoding
{
    class BlokFramesDecoder : IMessagesDecoder
    {
        public BlokFrame DecodeFrame(CanFrame Frame) { return BlokFrame.GetBlokFrame(Frame); }
    }
}