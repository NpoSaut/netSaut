using System;
using EventService.Interfaces;
using Saut.EventServices;

namespace EventService
{
    /// <summary>Слушатель, напрямую передающий событие</summary>
    /// <remarks>
    ///     Не рекомендуется использовать этот слушатель, т.к он не экранирует поток, возбуждающи событие: вся работа
    ///     происходит в нём
    /// </remarks>
    /// <typeparam name="TEvent">Тип события</typeparam>
    public class DirectEventListener<TEvent> : IConsumableEventListener<TEvent> where TEvent : Event
    {
        #region Rise

        /// <summary>Заставляет потребителя обработать насупившее событие</summary>
        /// <param name="NewEvent">Наступившее событие</param>
        public void ProcessEvent(Event NewEvent) { OnEventRaised(new EventRaisedArgs<TEvent>((TEvent)NewEvent)); }

        public event EventHandler<EventRaisedArgs<TEvent>> EventRaised;

        protected virtual void OnEventRaised(EventRaisedArgs<TEvent> E)
        {
            EventHandler<EventRaisedArgs<TEvent>> handler = EventRaised;
            if (handler != null) handler(this, E);
        }

        #endregion

        #region Dispose

        /// <summary>Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.</summary>
        public void Dispose() { OnDisposed(); }

        public event EventHandler Disposed;

        protected virtual void OnDisposed()
        {
            EventHandler handler = Disposed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        #endregion
    }

    public class DirectEventListenerFactory : IEventListenerFactory
    {
        public IConsumableEventListener<TEvent> GetEventListener<TEvent>() where TEvent : Event { return new DirectEventListener<TEvent>(); }
    }
}
