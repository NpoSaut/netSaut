using System;
using System.Collections.Generic;
using System.Linq;
using Saut.EventServices;

namespace EventService.Interfaces
{
    public class InterlockedConsumersContainerDecorator : IConsumersContainer
    {
        private readonly IConsumersContainer _container;
        private readonly object _locker = new object();
        public InterlockedConsumersContainerDecorator(IConsumersContainer Container) { _container = Container; }

        /// <summary>Регистрирует потребителя</summary>
        /// <typeparam name="TEvent">Тип потребляемого события</typeparam>
        /// <param name="EventConsumer">Потребитель события</param>
        public void RegisterConsumer<TEvent>(IEventConsumer EventConsumer) where TEvent : Event
        {
            lock (_locker)
            {
                _container.RegisterConsumer<TEvent>(EventConsumer);
            }
        }

        /// <summary>Возвращает список всех потребителей для данного типа события</summary>
        /// <param name="Type">Тип события</param>
        /// <returns>Список потребителей события</returns>
        public IEnumerable<IEventConsumer> Of(Type Type)
        {
            lock (_locker)
            {
                return _container.Of(Type).ToList();
            }
        }
    }
}
