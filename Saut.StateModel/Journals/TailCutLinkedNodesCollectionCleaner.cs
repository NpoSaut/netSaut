using System.Collections.Generic;

namespace Saut.StateModel.Journals
{
    /// <summary>Инструмент по отчистке коллекции, отрезающий хвост за первым не актуальным элементом</summary>
    /// <typeparam name="TCollectionElementValue">Тип значения записи журнала</typeparam>
    public class TailCutLinkedNodesCollectionCleaner<TCollectionElementValue> : ILinkedNodesCollectionCleaner<TCollectionElementValue>
    {
        private readonly ITailDetectPolicy<TCollectionElementValue> _tailDetectPolicy;

        public TailCutLinkedNodesCollectionCleaner(ITailDetectPolicy<TCollectionElementValue> TailDetectPolicy) { _tailDetectPolicy = TailDetectPolicy; }

        /// <summary>Отчищает коллекцию записей от устаревших элементов</summary>
        /// <param name="Records">Коллекция записей</param>
        public void Cleanup(IEnumerable<ConcurrentLogNode<TCollectionElementValue>> Records)
        {
            ConcurrentLogNode<TCollectionElementValue> lastActualElement = _tailDetectPolicy.GetLastActualElement(Records);
            CutNext(lastActualElement);
        }

        /// <summary>Обрезает цепочку после указанного элемента</summary>
        /// <remarks>
        ///     После некоторых размышлений, пришёл к выводу, что не нужно производить рекурсивное разрушение цепочки, т.к.
        ///     сборщик мусора наверняка достаточно умный, чтобы понять, что все отрезанные объекты можно изничтожить, если на них
        ///     нет ссылки с головы.
        /// </remarks>
        /// <param name="Element">Элемент, после которого цепочка будет разорвана</param>
        private void CutNext(ConcurrentLogNode<TCollectionElementValue> Element)
        {
            if (Element == null) return;
            Element.Next = null;
        }
    }
}
