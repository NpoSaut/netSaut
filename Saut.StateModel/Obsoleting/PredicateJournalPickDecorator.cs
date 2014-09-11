using System;
using System.Collections.Generic;
using System.Linq;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Obsoleting
{
    /// <summary>Фильтрующий декоратор на журнальную выборку</summary>
    /// <typeparam name="TValue">Тип значений в журнале</typeparam>
    public class PredicateJournalPickDecorator<TValue> : IJournalPick<TValue>
    {
        private readonly Func<JournalRecord<TValue>, bool> _afterPredicate;
        private readonly IJournalPick<TValue> _basePick;
        private readonly Func<JournalRecord<TValue>, bool> _beforePredicate;

        /// <summary>Создаёт фильтрующий декоратор на журнальную выборку</summary>
        /// <param name="BasePick">Базовая выборка</param>
        /// <param name="AfterPredicate">Условие фильтра на события после указанного времени</param>
        /// <param name="BeforePredicate">Условие фильтра на события до указанного времени</param>
        public PredicateJournalPickDecorator(IJournalPick<TValue> BasePick,
                                             Func<JournalRecord<TValue>, bool> AfterPredicate,
                                             Func<JournalRecord<TValue>, bool> BeforePredicate)
        {
            _basePick = BasePick;
            _afterPredicate = AfterPredicate;
            _beforePredicate = BeforePredicate;
        }

        /// <summary>Последовательность записей после указанного времени (в порядке первый - ранний).</summary>
        public IEnumerable<JournalRecord<TValue>> RecordsAfter
        {
            get { return _basePick.RecordsAfter.TakeWhile(_afterPredicate); }
        }

        /// <summary>Последовательность записей до указанного времени (в порядке первый - поздний).</summary>
        public IEnumerable<JournalRecord<TValue>> RecordsBefore
        {
            get { return _basePick.RecordsBefore.TakeWhile(_beforePredicate); }
        }
    }
}