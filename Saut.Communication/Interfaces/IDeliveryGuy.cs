using BlokFrames;

namespace Saut.Communication.Interfaces
{
    /// <summary>Сервис обработки входящих сообщений</summary>
    public interface IDeliveryGuy
    {
        /// <summary>Доставляет сообщение всем обработчикам</summary>
        /// <param name="Message">Свеже полученное сообщение</param>
        void DeliverMessage(BlokFrame Message);
    }
}
