using Saut.EventServices;

namespace EventService.Interfaces
{
    public interface IEventExpectantFactory
    {
        IConsumableEventExpectant<TEvent> GetEventExpectant<TEvent>() where TEvent : Event;
    }
}
