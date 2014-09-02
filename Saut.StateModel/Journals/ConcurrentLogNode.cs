namespace Saut.StateModel.Journals
{
    public class ConcurrentLogNode<TItem>
    {
        public readonly TItem Item;
        public ConcurrentLogNode<TItem> Next;

        public ConcurrentLogNode(TItem Item) { this.Item = Item; }

        public override string ToString() { return string.Format("{0} -> {1}", Item, Next != null ? "*" : "-"); }
    }
}
