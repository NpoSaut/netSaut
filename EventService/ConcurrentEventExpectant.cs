using System;
using System.Collections.Concurrent;
using System.Threading;
using EventService.Interfaces;
using Saut.EventServices;

namespace EventService
{
    public class ConcurrentEventExpectant<TEvent> : IConsumableEventExpectant<TEvent> where TEvent : Event
    {
        private readonly ConcurrentQueue<TEvent> _eventsQueue = new ConcurrentQueue<TEvent>();

        /// <summary>Заставляет потребителя обработать насупившее событие</summary>
        /// <param name="NewEvent">Наступившее событие</param>
        public void ProcessEvent(Event NewEvent) { _eventsQueue.Enqueue((TEvent)NewEvent); }

        public event EventHandler Disposed;

        /// <summary>Блокирует выполнение до наступления указанного события</summary>
        /// <returns>Первое наступившее события</returns>
        public TEvent Expect() { return Expect(TimeSpan.FromMilliseconds(-1)); }

        /// <summary>Блокирует выполнение до наступления указанного события (не более указанного таймаута)</summary>
        /// <param name="Timeout"></param>
        /// <exception cref="TimeoutException">Вышло время ожидания события</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="Timeout"/> является отрицательным числом отличный значение -1 миллисекунд, которое представляет неограниченное время ожидания - или - время ожидания больше MaxValue.</exception>
        /// <returns>Первое наступившее события</returns>
        public TEvent Expect(TimeSpan Timeout)
        {
            TEvent expectedEvent = null;
            SpinWait.SpinUntil(() => _eventsQueue.TryDequeue(out expectedEvent), Timeout);
            return expectedEvent;
        }

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
