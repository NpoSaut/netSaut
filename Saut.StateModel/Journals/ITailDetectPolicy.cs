using System.Collections.Generic;

namespace Saut.StateModel.Journals
{
    /// <summary>Политика выбора последнего актуального элемента в связанной коллекции</summary>
    /// <typeparam name="TCollectionElementValue">Тип элемента в связанной коллекции</typeparam>
    public interface ITailDetectPolicy<TCollectionElementValue>
    {
        /// <summary>Ищет последний актуальный элемент в коллекции</summary>
        /// <param name="Records">Коллекция для усечения</param>
        /// <returns>Элемент, после которого можно производить усечение коллекции</returns>
        ConcurrentLogNode<TCollectionElementValue> GetLastActualElement(IEnumerable<ConcurrentLogNode<TCollectionElementValue>> Records);
    }
}
