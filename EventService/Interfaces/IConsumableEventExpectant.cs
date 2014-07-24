using Saut.EventServices;

namespace EventService.Interfaces
{
    public interface IConsumableEventExpectant<TEvent> : IEventExpectant<TEvent>, IEventConsumer
        where TEvent : Event { }
}
