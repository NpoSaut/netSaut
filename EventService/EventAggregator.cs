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
        public void RaiseEvent(Event NewEvent)
        {
            foreach (IEventConsumer consumer in _consumers.Of(NewEvent.GetType()))
                consumer.ProcessEvent(NewEvent);
        }

        /// <summary>Создаёт и регистрирует прослушивателя событий</summary>
        /// <typeparam name="TEvent">Тип отлавливаемых событий</typeparam>
        /// <returns>Сконфигурированный прослушиватель</returns>
        /// <remarks>
        ///     Прослушиватель полезен, если функциональный класс заточен на обработку определённого типа сообщений в
        ///     отдельном потоке.
        /// </remarks>
        public IEventListener<TEvent> GetEventListener<TEvent>() where TEvent : Event
        {
            IConsumableEventListener<TEvent> listener = _listenerFactory.GetEventListener<TEvent>();
            _consumers.RegisterConsumer<TEvent>(listener);
            return listener;
        }

        /// <summary>Создаёт и регистрирует ожидателя сообщений (<see cref="IEventExpectant{TEvent}" />)</summary>
        /// <typeparam name="TEvent">Тип ожидаемых сообщений</typeparam>
        /// <returns>Сконфигурированный <see cref="IEventExpectant{TEvent}" /></returns>
        /// <remarks>
        ///     Позволяет заблокироваться в ожидании указанного типа события. Поток, создающий это событие не будет задержан
        ///     обработкой события
        /// </remarks>
        public IEventExpectant<TEvent> GetEventExpectant<TEvent>() where TEvent : Event
        {
            IConsumableEventExpectant<TEvent> expectant = _expectantFactory.GetEventExpectant<TEvent>();
            _consumers.RegisterConsumer<TEvent>(expectant);
            return expectant;
        }
    }
}
