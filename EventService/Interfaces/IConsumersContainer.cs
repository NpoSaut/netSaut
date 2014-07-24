using System;
using System.Collections.Generic;
using Saut.EventServices;

namespace EventService.Interfaces
{
    public interface IConsumersContainer
    {
        /// <summary>Регистрирует потребителя</summary>
        /// <typeparam name="TEvent">Тип потребляемого события</typeparam>
        /// <param name="EventConsumer">Потребитель события</param>
        void RegisterConsumer<TEvent>(IEventConsumer EventConsumer) where TEvent : Event;

        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        /// <summary>Возвращает список всех потребителей для данного типа события</summary>
        /// <param name="Type">Тип события</param>
        /// <returns>Список потребителей события</returns>
        IEnumerable<IEventConsumer> Of(Type Type);
    }
}
