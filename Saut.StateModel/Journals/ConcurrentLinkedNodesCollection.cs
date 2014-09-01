using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    public class ConcurrentLinkedNodesCollection<TValue> : IEnumerable<ConcurrentLogNode<TValue>>
    {
        /// <summary>Наиболее актуальное значение в журнале</summary>
        private ConcurrentLogNode<TValue> _head;

        public IEnumerator<ConcurrentLogNode<TValue>> GetEnumerator() { return new LinkedNodesEnumerator(_head); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /// <summary>Пытается не блокирующим образом вставить запись в коллекцию</summary>
        /// <param name="NewJournalNode">Вставляемая запись</param>
        /// <param name="Target">
        ///     Запись, после которой нужно вставить новую или null, если новую запись нужно вставить в начало
        ///     коллекции
        /// </param>
        /// <returns>True, если запись была успешно добавлена в коллекцию, False в противном случае</returns>
        public bool TryInsert(ConcurrentLogNode<TValue> NewJournalNode, ConcurrentLogNode<TValue> Target)
        {
            return
                Target != null
                    ? TryInsertInSequence(NewJournalNode, Target)
                    : TryInsertAsHead(NewJournalNode);
        }

        /// <summary>Пытается не блокирующим образом вставить запись в середину коллекцию</summary>
        /// <param name="NewJournalNode">Вставляемая запись</param>
        /// <param name="Target">Запись, после которой нужно вставить новую</param>
        /// <returns>True, если запись была успешно добавлена в коллекцию, False в противном случае</returns>
        private bool TryInsertInSequence(ConcurrentLogNode<TValue> NewJournalNode, ConcurrentLogNode<TValue> Target)
        {
            ConcurrentLogNode<TValue> targetsNext = Target.Next;
            NewJournalNode.Next = targetsNext;
            return Interlocked.CompareExchange(ref Target.Next, NewJournalNode, targetsNext) == targetsNext;
        }

        /// <summary>Пытается не блокирующим образом вставить запись в начало коллекции</summary>
        /// <param name="NewJournalNode">Вставляемая запись</param>
        /// <returns>True, если запись была успешно добавлена в коллекцию, False в противном случае</returns>
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

            /// <summary>Перемещает перечислитель к следующему элементу коллекции.</summary>
            /// <returns>
            ///     Значение true, если перечислитель был успешно перемещён к следующему элементу; значение false, если
            ///     перечислитель достиг конца коллекции.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">Коллекция была изменена после создания перечислителя.</exception>
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
