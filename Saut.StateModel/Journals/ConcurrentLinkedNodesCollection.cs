using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Saut.StateModel.Journals
{
    public class ConcurrentLinkedNodesCollection<TItem> : IConcurrentLinkedCollection<TItem>
    {
        /// <summary>Наиболее актуальное значение в журнале</summary>
        private ConcurrentLogNode<TItem> _head;

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /// <summary>Пытается не блокирующим образом вставить запись в коллекцию</summary>
        /// <param name="Item">Вставляемый элемент</param>
        /// <param name="Target">
        ///     Запись, после которой нужно вставить новую или null, если новую запись нужно вставить в начало
        ///     коллекции
        /// </param>
        /// <returns>True, если запись была успешно добавлена в коллекцию, False в противном случае</returns>
        public bool TryInsert(TItem Item, ConcurrentLogNode<TItem> Target)
        {
            var newJournalNode = new ConcurrentLogNode<TItem>(Item);
            return
                Target != null
                    ? TryInsertInSequence(newJournalNode, Target)
                    : TryInsertAsHead(newJournalNode);
        }

        public IEnumerator<ConcurrentLogNode<TItem>> GetEnumerator() { return new LinkedNodesEnumerator(_head); }

        /// <summary>Пытается не блокирующим образом вставить запись в середину коллекцию</summary>
        /// <param name="NewJournalNode">Вставляемая запись</param>
        /// <param name="Target">Запись, после которой нужно вставить новую</param>
        /// <returns>True, если запись была успешно добавлена в коллекцию, False в противном случае</returns>
        private bool TryInsertInSequence(ConcurrentLogNode<TItem> NewJournalNode, ConcurrentLogNode<TItem> Target)
        {
            ConcurrentLogNode<TItem> targetsNext = Target.Next;
            NewJournalNode.Next = targetsNext;
            return Interlocked.CompareExchange(ref Target.Next, NewJournalNode, targetsNext) == targetsNext;
        }

        /// <summary>Пытается не блокирующим образом вставить запись в начало коллекции</summary>
        /// <param name="NewJournalNode">Вставляемая запись</param>
        /// <returns>True, если запись была успешно добавлена в коллекцию, False в противном случае</returns>
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
