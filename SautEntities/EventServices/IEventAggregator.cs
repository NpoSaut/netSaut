namespace Saut.EventServices
{
    /// <summary>Агрегатор событий</summary>
    public interface IEventAggregator
    {
        /// <summary>Уведомляет о наступлении событие</summary>
        /// <param name="NewEvent">Наступившее событие</param>
        void RaiseEvent(Event NewEvent);

        /// <summary>Создаёт и регистрирует прослушивателя событий</summary>
        /// <typeparam name="TEvent">Тип отлавливаемых событий</typeparam>
        /// <returns>Сконфигурированный прослушиватель</returns>
        /// <remarks>
        ///     Прослушиватель полезен, если функциональный класс заточен на обработку определённого типа сообщений в
        ///     отдельном потоке.
        /// </remarks>
        IEventListener<TEvent> GetEventListener<TEvent>() where TEvent : Event;

        /// <summary>Создаёт и регистрирует ожидателя сообщений (<see cref="IEventExpectant{TEvent}" />)</summary>
        /// <typeparam name="TEvent">Тип ожидаемых сообщений</typeparam>
        /// <returns>Сконфигурированный <see cref="IEventExpectant{TEvent}" /></returns>
        /// <remarks>
        ///     Позволяет заблокироваться в ожидании указанного типа события. Поток, создающий это событие не будет задержан
        ///     обработкой события
        /// </remarks>
        IEventExpectant<TEvent> GetEventExpectant<TEvent>() where TEvent : Event;
    }
}
