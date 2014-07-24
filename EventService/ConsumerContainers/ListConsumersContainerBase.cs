using System;
using System.Collections.Generic;
using System.Linq;
using EventService.Interfaces;
using Saut.EventServices;

namespace EventService.ConsumerContainers
{
    /// <summary>Базовый класс контейнера потребителей событий</summary>
    /// <remarks>
    ///     Обеспечивает логику регистрации, и удаления потребителей; логику выбора потребителей, соответствующих
    ///     заданному событию. Упускает реализацию хранения списка потребителей
    /// </remarks>
    public abstract class ListConsumersContainerBase : IConsumersContainer
    {
        public void RegisterConsumer<TEvent>(IEventConsumer EventConsumer) where TEvent : Event
        {
            EventConsumer.Disposed += EventConsumerOnDisposed;
            AddTarget(new EventTarget(typeof (TEvent), EventConsumer));
        }

        public IEnumerable<IEventConsumer> Of(Type EventType)
        {
            return EnumerateTargets().Where(t => t.CanConsumeEventType(EventType)).Select(t => t.Consumer);
        }

        /// <summary>Добавляет потребителя в список</summary>
        protected abstract void AddTarget(EventTarget Target);

        /// <summary>Убирает потребителя из списка</summary>
        protected abstract void RemoveTarget(EventTarget Target);

        /// <summary>Перечисляет всех потребителей</summary>
        protected abstract IEnumerable<EventTarget> EnumerateTargets();

        private void ReleaseConsumer(IEventConsumer EventConsumer) { RemoveTarget(EnumerateTargets().Single(t => t.Consumer == EventConsumer)); }

        private void EventConsumerOnDisposed(object Sender, EventArgs Args) { ReleaseConsumer((IEventConsumer)Sender); }

        protected class EventTarget
        {
            public EventTarget(Type ConsumedEventType, IEventConsumer Consumer)
            {
                this.ConsumedEventType = ConsumedEventType;
                this.Consumer = Consumer;
            }

            public Type ConsumedEventType { get; private set; }
            public IEventConsumer Consumer { get; private set; }

            public bool CanConsumeEventType(Type EventType) { return EventType == ConsumedEventType || EventType.IsSubclassOf(ConsumedEventType); }
        }
    }
}
