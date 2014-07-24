using System;

namespace Saut.EventServices
{
    /// <summary>Потребитель информации о наступивших событиях</summary>
    public interface IEventConsumer : IDisposable
    {
        /// <summary>Заставляет потребителя обработать насупившее событие</summary>
        /// <param name="NewEvent">Наступившее событие</param>
        void ProcessEvent(Event NewEvent);

        /// <summary>
        ///     Возникает при закрытии потребителя. После возникновения этого события можно не уведомлять потребителя о
        ///     наступающих событиях
        /// </summary>
        event EventHandler Disposed;
    }
}
