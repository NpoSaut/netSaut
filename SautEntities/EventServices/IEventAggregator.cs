using System;
using System.Threading.Tasks;

namespace Saut.EventServices
{
    /// <summary>Агрегатор событий</summary>
    public interface IEventAggregator
    {
        /// <summary>Уведомляет о наступлении событие</summary>
        /// <param name="NewEvent">Наступившее событие</param>
        void RaiseEvent(Event NewEvent);

        IEventListener<TEvent> GetEventListener<TEvent>() where TEvent : Event;
        IEventExpectant<TEvent> GetEventExpectant<TEvent>() where TEvent : Event;
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
            Aggregator.GetEventExpectant<MyEvent>().Expect();
            Aggregator.RaiseEvent(new MyEvent("loh"));
        }

        public void Dispose() { _listener.Dispose(); }

        public async void ghovnar()
        {
            var evx = _aggregator.ExpectEvent<Event>();

            using (IEventExpectant<MyEvent> expector = _aggregator.GetEventExpectant<MyEvent>())
            {
                Task<MyEvent> xx = expector.ExpectAsync();
                MyEvent ev = await xx;
            }
        }

        private void OnEventRaised(object Sender, EventRaisedArgs<MyEvent> EventRaisedArgs) { }

        private class MyEvent : Event
        {
            public MyEvent(string Name) { this.Name = Name; }
            public string Name { get; private set; }
        }
    }
}
