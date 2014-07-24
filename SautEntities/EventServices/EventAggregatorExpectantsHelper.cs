using System;

namespace Saut.EventServices
{
    /// <summary>Класс с методами расширения для более лёгкого ожидания событий</summary>
    public static class EventAggregatorExpectantsHelper
    {
        /// <summary>Ожидает первого (одного) же события указанного типа</summary>
        /// <typeparam name="TEvent">Тип ожидаемого события</typeparam>
        /// <param name="Aggregator">Агрегатор событий</param>
        /// <returns>Первое наступившее событие указанного типа</returns>
        /// <remarks>
        ///     <para>
        ///         Этот метод позволяет по-быстрому получить первое пришедшее событие указанного типа, после чего объект,
        ///         ожидающий событие будет уничтожен.
        ///     </para>
        ///     <para>
        ///         Этот метод годится только для единичного ожидания события. Если требуется отслеживать повторяющиеся события,
        ///         то лучше получить объект <see cref="IEventExpectant{TEvent}" />, вызвав соответствующий метод
        ///         <see cref="IEventAggregator" />. <see cref="IEventExpectant{TEvent}" /> будет сохранять все события в очередь,
        ///         и ни одно из них не будет упущено.
        ///     </para>
        /// </remarks>
        public static TEvent ExpectEvent<TEvent>(this IEventAggregator Aggregator) where TEvent : Event
        {
            using (IEventExpectant<TEvent> expectant = Aggregator.GetEventExpectant<TEvent>())
            {
                return expectant.Expect();
            }
        }

        /// <summary>Ожидает первого (одного) же события указанного типа с ограничением максимального времени ожидания</summary>
        /// <typeparam name="TEvent">Тип ожидаемого события</typeparam>
        /// <param name="Aggregator">Агрегатор событий</param>
        /// <param name="Timeout">Максимальное время ожидания</param>
        /// <returns>Первое наступившее событие указанного типа</returns>
        /// <remarks>
        ///     <para>
        ///         Этот метод позволяет по-быстрому получить первое пришедшее событие указанного типа, после чего объект,
        ///         ожидающий событие будет уничтожен.
        ///     </para>
        ///     <para>
        ///         Этот метод годится только для единичного ожидания события. Если требуется отслеживать повторяющиеся события,
        ///         то лучше получить объект <see cref="IEventExpectant{TEvent}" />, вызвав соответствующий метод
        ///         <see cref="IEventAggregator" />. <see cref="IEventExpectant{TEvent}" /> будет сохранять все события в очередь,
        ///         и ни одно из них не будет упущено.
        ///     </para>
        /// </remarks>
        /// <exception cref="TimeoutException">Вышло время ожидания наступления события</exception>
        public static TEvent ExpectEvent<TEvent>(this IEventAggregator Aggregator, TimeSpan Timeout) where TEvent : Event
        {
            using (IEventExpectant<TEvent> expectant = Aggregator.GetEventExpectant<TEvent>())
            {
                return expectant.Expect(Timeout);
            }
        }
    }
}