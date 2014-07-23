using Saut.EventServices;

namespace EventService.Interfaces
{
    public interface IEventListenerFactory
    {
        IConsumableEventListener<TEvent> GetEventListener<TEvent>() where TEvent : Event;
    }
}
