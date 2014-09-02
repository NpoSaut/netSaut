using System.Collections.Generic;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Journals
{
    public interface IConcurrentLinkedCollection<TItem> : IEnumerable<ConcurrentLogNode<TItem>>
    {
        /// <summary>Пытается не блокирующим образом вставить запись в коллекцию</summary>
        /// <param name="Item">Вставляемый элемент</param>
        /// <param name="Target">
        ///     Запись, после которой нужно вставить новую или null, если новую запись нужно вставить в начало
        ///     коллекции
        /// </param>
        /// <returns>True, если запись была успешно добавлена в коллекцию, False в противном случае</returns>
        bool TryInsert(TItem Item, ConcurrentLogNode<TItem> Target);
    }
}