using System;
using System.Collections.Generic;
using System.Threading;

namespace Saut.StateModel.Journals
{
    /// <summary>Декоратор, позволяющий потокобезопасно пропускать некоторые запросы на отчистку</summary>
    /// <typeparam name="TCollectionElementValue">Тип элемента в отчищаемой коллекции</typeparam>
    public class SkippingLinkedNodesCollectionCleanerDecorator<TCollectionElementValue> : ILinkedNodesCollectionCleaner<TCollectionElementValue>
    {
        private readonly ILinkedNodesCollectionCleaner<TCollectionElementValue> _baseCleaner;

        private readonly int _skipCycle;
        private int _counter;

        /// <summary>Создаёт декоратор, позволяющий потокобезопасно пропускать некоторые запросы на отчистку</summary>
        /// <param name="BaseCleaner">Базовый инструмент по отчистке коллекции</param>
        /// <param name="SkipCycle">Количество игнорируемых запросов (0 -- выполнять каждый раз)</param>
        public SkippingLinkedNodesCollectionCleanerDecorator(ILinkedNodesCollectionCleaner<TCollectionElementValue> BaseCleaner, int SkipCycle)
        {
            if (SkipCycle < 0) throw new ArgumentException("Количество пропусков должно быть не отрицательным", "SkipCycle");
            _baseCleaner = BaseCleaner;
            _skipCycle = SkipCycle;
        }

        /// <summary>Отчищает коллекцию записей от устаревших элементов</summary>
        /// <param name="Collection">Коллекция записей</param>
        public void Cleanup(IEnumerable<ConcurrentLogNode<TCollectionElementValue>> Collection)
        {
            Interlocked.Increment(ref _counter);
            if (Interlocked.CompareExchange(ref _counter, 0, _skipCycle) == _skipCycle)
                _baseCleaner.Cleanup(Collection);
        }
    }
}
