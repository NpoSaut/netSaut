using System.Collections.Generic;

namespace EventService.ConsumerContainers
{
    /// <summary>Простой список потребителей</summary>
    public class ListConsumersContainer : ListConsumersContainerBase
    {
        private readonly IList<EventTarget> _targets = new List<EventTarget>();

        protected override void AddTarget(EventTarget Target) { _targets.Add(Target); }
        protected override void RemoveTarget(EventTarget Target) { _targets.Remove(Target); }
        protected override IEnumerable<EventTarget> EnumerateTargets() { return _targets; }
    }
}
