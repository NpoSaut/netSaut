using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    public class ConcurrentJournal<TValue> : IJournal<TValue>
    {
        private readonly NodesCollection _records = new NodesCollection();

        /// <summary>Все записи в журнале в порядке устаревания (новые - первыми).</summary>
        public IEnumerable<JournalRecord<TValue>> Records
        {
            get { return _records.Select(r => r.Item); }
        }

        /// <summary>Добавляет запись в журнал.</summary>
        /// <param name="Value">Значение.</param>
        /// <param name="RecordTime">Время.</param>
        public void AddRecord(TValue Value, DateTime RecordTime)
        {
            var newRecord = new JournalRecord<TValue>(RecordTime, Value);

            bool insertionSuccessed;
            do
            {
                ConcurrentLogNode target = _records.TakeWhile(r => r.Item.Time > RecordTime).LastOrDefault();
                if (target != null)
                {
                    var newJournalNode = new ConcurrentLogNode(newRecord);
                    var targetsNext = target.Next;
                    newJournalNode.Next = targetsNext;
                    target.Next = newJournalNode;
                    insertionSuccessed = Interlocked.CompareExchange(ref target.Next, newJournalNode, targetsNext) == targetsNext;
                }
                else
                {
                    var newJournalNode = new ConcurrentLogNode(newRecord);
                    var oldHead = _records._head;
                    newJournalNode.Next = oldHead;
                    insertionSuccessed = Interlocked.CompareExchange(ref _records._head, newJournalNode, oldHead) == oldHead;
                }
            } while (!insertionSuccessed);
        }

        private class ConcurrentLogNode
        {
            public readonly JournalRecord<TValue> Item;
            public ConcurrentLogNode Next;

            public ConcurrentLogNode(JournalRecord<TValue> Item) { this.Item = Item; }

            public void Append(ConcurrentLogNode NewNode)
            {
                bool successed;
                do
                {
                    NewNode.Next = Next;
                    successed = NewNode.Next == Interlocked.CompareExchange(ref Next, NewNode, NewNode.Next);
                } while (!successed);
            }
        }

        private class LinkedNodesEnumerator : IEnumerator<ConcurrentLogNode>
        {
            private readonly ConcurrentLogNode _headNode;
            private ConcurrentLogNode _currentNode;

            public LinkedNodesEnumerator(ConcurrentLogNode HeadNode)
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
                _currentNode = _currentNode.Next;
                return (_currentNode != null);
            }

            public void Reset() { _currentNode = _headNode; }

            public ConcurrentLogNode Current
            {
                get { return _currentNode; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        private class NodesCollection : IEnumerable<ConcurrentLogNode>
        {
            /// <summary>Наиболее актуальное значение в журнале</summary>
            public ConcurrentLogNode _head;

            public IEnumerator<ConcurrentLogNode> GetEnumerator() { return new LinkedNodesEnumerator(_head); }
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }
    }
}
