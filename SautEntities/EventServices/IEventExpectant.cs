﻿using System;
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
        /// <param name="Timeout"></param>
        /// <exception cref="TimeoutException">Вышло время ожидания события</exception>
        /// <returns>Первое наступившее события</returns>
        TEvent Expect(TimeSpan Timeout);
    }

    public static class AsyncEventExpectation
    {
        public static Task<TEvent> ExpectAsync<TEvent>(this IEventExpectant<TEvent> Expectant) where TEvent : Event
        {
            return Task.Run(() => Expectant.Expect());
        }

        public static Task<TEvent> ExpectAsync<TEvent>(this IEventExpectant<TEvent> Expectant, TimeSpan Timeout) where TEvent : Event
        {
            return Task.Run(() => Expectant.Expect(Timeout));
        }
    }
}