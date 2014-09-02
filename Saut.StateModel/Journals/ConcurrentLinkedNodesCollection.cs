using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Saut.StateModel.Journals
{
    public class ConcurrentLinkedNodesCollection<TItem> : IConcurrentLinkedCollection<TItem>
    {
        /// <summary>�������� ���������� �������� � �������</summary>
        private ConcurrentLogNode<TItem> _head;

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /// <summary>�������� �� ����������� ������� �������� ������ � ���������</summary>
        /// <param name="Item">����������� �������</param>
        /// <param name="Target">
        ///     ������, ����� ������� ����� �������� ����� ��� null, ���� ����� ������ ����� �������� � ������
        ///     ���������
        /// </param>
        /// <returns>True, ���� ������ ���� ������� ��������� � ���������, False � ��������� ������</returns>
        public bool TryInsert(TItem Item, ConcurrentLogNode<TItem> Target)
        {
            var newJournalNode = new ConcurrentLogNode<TItem>(Item);
            return
                Target != null
                    ? TryInsertInSequence(newJournalNode, Target)
                    : TryInsertAsHead(newJournalNode);
        }

        public IEnumerator<ConcurrentLogNode<TItem>> GetEnumerator() { return new LinkedNodesEnumerator(_head); }

        /// <summary>�������� �� ����������� ������� �������� ������ � �������� ���������</summary>
        /// <param name="NewJournalNode">����������� ������</param>
        /// <param name="Target">������, ����� ������� ����� �������� �����</param>
        /// <returns>True, ���� ������ ���� ������� ��������� � ���������, False � ��������� ������</returns>
        private bool TryInsertInSequence(ConcurrentLogNode<TItem> NewJournalNode, ConcurrentLogNode<TItem> Target)
        {
            ConcurrentLogNode<TItem> targetsNext = Target.Next;
            NewJournalNode.Next = targetsNext;
            return Interlocked.CompareExchange(ref Target.Next, NewJournalNode, targetsNext) == targetsNext;
        }

        /// <summary>�������� �� ����������� ������� �������� ������ � ������ ���������</summary>
        /// <param name="NewJournalNode">����������� ������</param>
        /// <returns>True, ���� ������ ���� ������� ��������� � ���������, False � ��������� ������</returns>
        private bool TryInsertAsHead(ConcurrentLogNode<TItem> NewJournalNode)
        {
            ConcurrentLogNode<TItem> oldHead = _head;
            NewJournalNode.Next = oldHead;
            return Interlocked.CompareExchange(ref _head, NewJournalNode, oldHead) == oldHead;
        }

        private class LinkedNodesEnumerator : IEnumerator<ConcurrentLogNode<TItem>>
        {
            private readonly ConcurrentLogNode<TItem> _headNode;
            private ConcurrentLogNode<TItem> _currentNode;

            public LinkedNodesEnumerator(ConcurrentLogNode<TItem> HeadNode)
            {
                _headNode = HeadNode;
                Reset();
            }

            public void Dispose() { }

            /// <summary>���������� ������������� � ���������� �������� ���������.</summary>
            /// <returns>
            ///     �������� true, ���� ������������� ��� ������� ��������� � ���������� ��������; �������� false, ����
            ///     ������������� ������ ����� ���������.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">��������� ���� �������� ����� �������� �������������.</exception>
            public bool MoveNext()
            {
                if (_currentNode == null) return false;
                _currentNode = _currentNode.Next;
                return (_currentNode != null);
            }

            public void Reset() { _currentNode = new ConcurrentLogNode<TItem>(default(TItem)) { Next = _headNode }; }

            public ConcurrentLogNode<TItem> Current
            {
                get { return _currentNode; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
