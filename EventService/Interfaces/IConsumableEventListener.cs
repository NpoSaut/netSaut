using Saut.EventServices;

namespace EventService.Interfaces
{
    public interface IConsumableEventListener<TEvent> : IEventListener<TEvent>, IEventConsumer where TEvent : Event { }
}
