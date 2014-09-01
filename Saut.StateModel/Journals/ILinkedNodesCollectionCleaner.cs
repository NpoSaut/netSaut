using System.Collections.Generic;

namespace Saut.StateModel.Journals
{
    /// <summary>Инструмент по отчистке коллекции записей в журнале от неактуальных элементов</summary>
    /// <typeparam name="TCollectionElementValue">Тип значения записи журнала</typeparam>
    public interface ILinkedNodesCollectionCleaner<TCollectionElementValue>
    {
        /// <summary>Отчищает коллекцию записей от устаревших элементов</summary>
        /// <param name="Records">Коллекция записей</param>
        void Cleanup(IEnumerable<ConcurrentLogNode<TCollectionElementValue>> Records);
    }
}
