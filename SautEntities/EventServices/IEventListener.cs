using System;

namespace Saut.EventServices
{
    public interface IEventListener<TEvent> : IDisposable
        where TEvent : Event
    {
        event EventHandler<EventRaisedArgs<TEvent>> EventRaised;
    }

    public class EventRaisedArgs<TEvent> : EventArgs where TEvent : Event
    {
        public EventRaisedArgs(TEvent Event) { this.Event = Event; }
        public TEvent Event { get; private set; }
    }
}
