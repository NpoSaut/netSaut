using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    public class ConcurrentLogNode<TValue>
    {
        public readonly JournalRecord<TValue> Item;
        public ConcurrentLogNode<TValue> Next;

        public ConcurrentLogNode(JournalRecord<TValue> Item) { this.Item = Item; }

        public override string ToString() { return string.Format("{0} -> {1}", Item, Next != null ? "*" : "-"); }
    }
}