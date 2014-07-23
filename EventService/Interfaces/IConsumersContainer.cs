using Saut.EventServices;

namespace EventService.Interfaces
{
    public interface IConsumersContainer
    {
        void RegisterConsumer(IEventConsumer EventConsumer);
    }
}
