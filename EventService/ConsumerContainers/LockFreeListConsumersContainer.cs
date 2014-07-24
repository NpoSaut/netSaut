using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EventService.ConsumerContainers
{
    /// <summary>Неблокирующий, потокобезопасный контейнер потребителей событий</summary>
    /// <remarks>
    ///     <para>При перечислении потребителей не происходит блокировки, выдаётся коллекция, актуальная на момент запроса.</para>
    ///     <para>При добавлении/удалении потребителя происходит Interlocked-замена списка.</para>
    ///     <para>
    ///         Таким образом, чтение списка всегда будет происходить быстро, а его модификация может занять неопределённое
    ///         время, в случае, если два потока будут синхронно пытаться его модифицировать.
    ///     </para>
    /// </remarks>
    public class LockFreeListConsumersContainer : ListConsumersContainerBase
    {
        private IList<EventTarget> _targets = new EventTarget[0];

        /// <summary>Производит потокобезопасное изменение списка потребителей событий</summary>
        /// <param name="ModifyFunc">Функция-модификатор списка событий</param>
        private void SafeModifyTargets(Func<IList<EventTarget>, IList<EventTarget>> ModifyFunc)
        {
            bool successed;
            do
            {
                IList<EventTarget> oldList = _targets;
                IList<EventTarget> newList = ModifyFunc(oldList);
                successed = Interlocked.CompareExchange(ref _targets, newList, oldList) == oldList;
            } while (successed);
        }

        /// <summary>Добавляет потребителя в список</summary>
        protected override void AddTarget(EventTarget Target) { SafeModifyTargets(l => l.Concat(new[] { Target }).ToList()); }

        /// <summary>Убирает потребителя из списка</summary>
        protected override void RemoveTarget(EventTarget Target) { SafeModifyTargets(l => l.Where(t => t != Target).ToList()); }

        /// <summary>Перечисляет всех потребителей</summary>
        protected override IEnumerable<EventTarget> EnumerateTargets() { return _targets; }
    }
}
