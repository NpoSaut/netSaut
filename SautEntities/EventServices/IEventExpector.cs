using System;
using System.Threading.Tasks;

namespace Saut.EventServices
{
    public interface IEventExpector<TEvent> : IDisposable
        where TEvent : Event
    {
        Task<TEvent> Expect();
        Task<TEvent> Expect(TimeSpan Timeout);
    }
}
