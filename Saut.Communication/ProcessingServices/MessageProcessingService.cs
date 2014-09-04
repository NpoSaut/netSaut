using BlokFrames;
using Communications;
using Communications.Can;
using Saut.Communication.Interfaces;

namespace Saut.Communication.ProcessingServices
{
    /// <summary>Простой сервис обработки сообщений</summary>
    /// <remarks>Висит в бесконечной петле, пытаясь принять и обработать все входящие сообщения</remarks>
    public class MessageProcessingService : IMessageProcessingService
    {
        private readonly IMessagesDecoder _decoder;
        private readonly IDeliveryGuy _deliveryGuy;
        private readonly ICanSocket _socket;

        public MessageProcessingService(ISocketSource<ICanSocket> SocketSource, IDeliveryGuy DeliveryGuy, IMessagesDecoder Decoder)
        {
            _socket = SocketSource.OpenSocket();
            _deliveryGuy = DeliveryGuy;
            _decoder = Decoder;
        }

        public void Dispose() { _socket.Dispose(); }

        public void Run()
        {
            foreach (CanFrame canFrame in _socket.Read())
            {
                BlokFrame message = _decoder.DecodeFrame(canFrame);
                _deliveryGuy.DeliverMessage(message);
            }
        }
    }
}
