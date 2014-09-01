using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    public class ConcurrentLinkedNodesCollection<TValue> : IEnumerable<ConcurrentLogNode<TValue>>
    {
        /// <summary>�������� ���������� �������� � �������</summary>
        private ConcurrentLogNode<TValue> _head;

        public IEnumerator<ConcurrentLogNode<TValue>> GetEnumerator() { return new LinkedNodesEnumerator(_head); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /// <summary>�������� �� ����������� ������� �������� ������ � ���������</summary>
        /// <param name="NewJournalNode">����������� ������</param>
        /// <param name="Target">
        ///     ������, ����� ������� ����� �������� ����� ��� null, ���� ����� ������ ����� �������� � ������
        ///     ���������
        /// </param>
        /// <returns>True, ���� ������ ���� ������� ��������� � ���������, False � ��������� ������</returns>
        public bool TryInsert(ConcurrentLogNode<TValue> NewJournalNode, ConcurrentLogNode<TValue> Target)
        {
            return
                Target != null
                    ? TryInsertInSequence(NewJournalNode, Target)
                    : TryInsertAsHead(NewJournalNode);
        }

        /// <summary>�������� �� ����������� ������� �������� ������ � �������� ���������</summary>
        /// <param name="NewJournalNode">����������� ������</param>
        /// <param name="Target">������, ����� ������� ����� �������� �����</param>
        /// <returns>True, ���� ������ ���� ������� ��������� � ���������, False � ��������� ������</returns>
        private bool TryInsertInSequence(ConcurrentLogNode<TValue> NewJournalNode, ConcurrentLogNode<TValue> Target)
        {
            ConcurrentLogNode<TValue> targetsNext = Target.Next;
            NewJournalNode.Next = targetsNext;
            return Interlocked.CompareExchange(ref Target.Next, NewJournalNode, targetsNext) == targetsNext;
        }

        /// <summary>�������� �� ����������� ������� �������� ������ � ������ ���������</summary>
        /// <param name="NewJournalNode">����������� ������</param>
        /// <returns>True, ���� ������ ���� ������� ��������� � ���������, False � ��������� ������</returns>
        private bool TryInsertAsHead(ConcurrentLogNode<TValue> NewJournalNode)
        {
            ConcurrentLogNode<TValue> oldHead = _head;
            NewJournalNode.Next = oldHead;
            return Interlocked.CompareExchange(ref _head, NewJournalNode, oldHead) == oldHead;
        }

        private class LinkedNodesEnumerator : IEnumerator<ConcurrentLogNode<TValue>>
        {
            private readonly ConcurrentLogNode<TValue> _headNode;
            private ConcurrentLogNode<TValue> _currentNode;

            public LinkedNodesEnumerator(ConcurrentLogNode<TValue> HeadNode)
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

            public void Reset() { _currentNode = new ConcurrentLogNode<TValue>(default(JournalRecord<TValue>)) { Next = _headNode }; }

            public ConcurrentLogNode<TValue> Current
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
