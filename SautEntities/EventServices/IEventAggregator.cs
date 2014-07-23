using System;
using System.Threading.Tasks;

namespace Saut.EventServices
{
    /// <summary>Менеджер событий</summary>
    public interface IEventAggregator
    {
        /// <summary>Уведомляет о наступлении событие</summary>
        /// <param name="NewEvent">Наступившее событие</param>
        void RaiseEvent(Event NewEvent);

        IEventListener<TEvent> GetEventListener<TEvent>() where TEvent : Event;
        IEventExpector<TEvent> GetEventExpector<TEvent>() where TEvent : Event;
    }

    public class EventConsumer : IDisposable
    {
        private readonly IEventAggregator _aggregator;
        // TODO: Я хочу, чтобы не пришлось вручную диспозить каждый токен

        private readonly IEventListener<MyEvent> _listener;

        public EventConsumer(IEventAggregator Aggregator)
        {
            _aggregator = Aggregator;
            (_listener = Aggregator.GetEventListener<MyEvent>()).EventRaised += OnEventRaised;
            Aggregator.GetEventExpector<MyEvent>().Expect();
            Aggregator.RaiseEvent(new MyEvent("loh"));
        }

        public async void ghovnar()
        {
            using (var expector = _aggregator.GetEventExpector<MyEvent>())
            {
                var xx = expector.Expect();
                var ev = await xx;
            }
        }

        public void Dispose() { _listener.Dispose(); }

        private void OnEventRaised(object Sender, EventRaisedArgs<MyEvent> EventRaisedArgs) { }

        private class MyEvent : Event
        {
            public MyEvent(string Name) { this.Name = Name; }
            public string Name { get; private set; }
        }
    }
}
