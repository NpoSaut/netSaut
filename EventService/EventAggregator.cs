using EventService.Interfaces;
using Saut.EventServices;

namespace EventService
{
    /// <summary>Простой агрегатор событий</summary>
    public class EventAggregator : IEventAggregator
    {
        private readonly IConsumersContainer _consumers;
        private readonly IEventExpectantFactory _expectantFactory;
        private readonly IEventListenerFactory _listenerFactory;

        public EventAggregator(IConsumersContainer Consumers, IEventExpectantFactory ExpectantFactory, IEventListenerFactory ListenerFactory)
        {
            _consumers = Consumers;
            _expectantFactory = ExpectantFactory;
            _listenerFactory = ListenerFactory;
        }

        /// <summary>Уведомляет о наступлении событие</summary>
        /// <param name="NewEvent">Наступившее событие</param>
        public void RaiseEvent(Event NewEvent) { }

        public IEventListener<TEvent> GetEventListener<TEvent>() where TEvent : Event
        {
            IConsumableEventListener<TEvent> listener = _listenerFactory.GetEventListener<TEvent>();
            _consumers.RegisterConsumer(listener);
            return listener;
        }

        public IEventExpectant<TEvent> GetEventExpector<TEvent>() where TEvent : Event
        {
            IConsumableEventExpectant<TEvent> expectant = _expectantFactory.GetEventExpectant<TEvent>();
            _consumers.RegisterConsumer(expectant);
            return expectant;
        }
    }
}
