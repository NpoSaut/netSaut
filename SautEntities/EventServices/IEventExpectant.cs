using System;
using System.Threading.Tasks;

namespace Saut.EventServices
{
    /// <summary>Объект, позволяющий ожидать наступления заданного события</summary>
    /// <typeparam name="TEvent">Тип ожидаемого события</typeparam>
    public interface IEventExpectant<TEvent> : IDisposable
        where TEvent : Event
    {
        /// <summary>Блокирует выполнение до наступления указанного события</summary>
        /// <returns>Первое наступившее события</returns>
        TEvent Expect();

        /// <summary>Блокирует выполнение до наступления указанного события (не более указанного таймаута)</summary>
        /// <param name="Timeout">Таймаут ожидания наступления события</param>
        /// <exception cref="TimeoutException">Вышло время ожидания события</exception>
        /// <returns>Первое наступившее события</returns>
        TEvent Expect(TimeSpan Timeout);
    }

    /// <summary>Класс-помощник, реализующий методы расширения для работы с ожидателем события в async-стиле</summary>
    public static class AsyncEventExpectation
    {
        /// <summary>Асинхронно ожидает наступления события указанного типа</summary>
        /// <typeparam name="TEvent">Тип ожидаемого события</typeparam>
        /// <param name="Expectant">Ожидатель события</param>
        /// <returns>Первое наступившее событие типа <typeparamref name="TEvent" /></returns>
        public static Task<TEvent> ExpectAsync<TEvent>(this IEventExpectant<TEvent> Expectant) where TEvent : Event
        {
            return Task.Run(() => Expectant.Expect());
        }

        /// <summary>Асинхронно ожидает наступления события указанного типа с указанным таймаутом</summary>
        /// <typeparam name="TEvent">Тип ожидаемого события</typeparam>
        /// <param name="Expectant">Ожидатель события</param>
        /// <param name="Timeout">Таймаут ожидания наступления события</param>
        /// <returns>Первое наступившее событие типа <typeparamref name="TEvent" /></returns>
        public static Task<TEvent> ExpectAsync<TEvent>(this IEventExpectant<TEvent> Expectant, TimeSpan Timeout) where TEvent : Event
        {
            return Task.Run(() => Expectant.Expect(Timeout));
        }
    }
}
