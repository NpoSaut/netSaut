using System;

namespace Saut.Communication.Interfaces
{
    /// <summary>Сервис приёма и обработки входящих сообщений</summary>
    public interface IMessageProcessingService : IDisposable
    {
        /// <summary>Запускает работу сервиса</summary>
        void Run();
    }
}
