using System;

namespace Saut.EventServices
{
    public interface IEventConsumer
    {
        void ProcessEvent(Event NewEvent);
        event EventHandler Disposed;
    }
}