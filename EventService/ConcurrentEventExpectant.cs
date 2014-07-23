using System;
using System.Threading.Tasks;
using EventService.Interfaces;
using Saut.EventServices;

namespace EventService
{
    public class ConcurrentEventExpectant<TEvent> : IConsumableEventExpectant<TEvent> where TEvent : Event
    {
        /// <summary>Заставляет потребителя обработать насупившее событие</summary>
        /// <param name="NewEvent">Наступившее событие</param>
        public void ProcessEvent(Event NewEvent) { throw new NotImplementedException(); }

        public event EventHandler Disposed;

        /// <summary>Блокирует выполнение до наступления указанного события</summary>
        /// <returns>Первое наступившее события</returns>
        public Task<TEvent> ExpectAsync() { return ExpectAsync(TimeSpan.Zero); }

        /// <summary>Блокирует выполнение до наступления указанного события (не более указанного таймаута)</summary>
        /// <param name="Timeout"></param>
        /// <exception cref="TimeoutException">Вышло время ожидания события</exception>
        /// <returns>Первое наступившее события</returns>
        public Task<TEvent> ExpectAsync(TimeSpan Timeout) { throw new NotImplementedException(); }

        /// <summary>Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.</summary>
        public void Dispose() { OnDisposed(); }

        private void OnDisposed()
        {
            EventHandler handler = Disposed;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }

    public class ConcurrentEventExpectantFactory : IEventExpectantFactory
    {
        public IConsumableEventExpectant<TEvent> GetEventExpectant<TEvent>() where TEvent : Event { return new ConcurrentEventExpectant<TEvent>(); }
    }
}
