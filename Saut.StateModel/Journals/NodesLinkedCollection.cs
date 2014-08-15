using System.Collections;
using System.Collections.Generic;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    public class NodesLinkedCollection<TValue> : IEnumerable<ConcurrentLogNode<TValue>>
    {
        /// <summary>�������� ���������� �������� � �������</summary>
        public ConcurrentLogNode<TValue> _head;

        public IEnumerator<ConcurrentLogNode<TValue>> GetEnumerator() { return new LinkedNodesEnumerator(_head); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

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