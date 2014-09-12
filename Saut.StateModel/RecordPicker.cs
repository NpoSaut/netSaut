using System;
using System.Collections.Generic;
using System.Linq;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel
{
    public class RecordPicker : IRecordPicker
    {
        /// <summary>Создаёт выборку из журнала в окрестности указанного времени.</summary>
        /// <typeparam name="TValue">Тип значений в журнале.</typeparam>
        /// <param name="Journal">Журнал, в котором будет выполняться поиск.</param>
        /// <param name="Time">Время, в окрестности которого нужно сделать выборку.</param>
        /// <returns>Выборку из журнала в окрестности указанного времени.</returns>
        public IJournalPick<TValue> PickRecords<TValue>(IJournal<TValue> Journal, DateTime Time)
        {
            return
                new JournalPick<TValue>(
                    Journal.Records.TakeWhile(r => r.Time > Time).Reverse(),
                    Journal.Records.SkipWhile(r => r.Time > Time));
        }

        public class JournalPick<TValue> : IJournalPick<TValue>
        {
            /// <summary>Инициализирует новый экземпляр класса <see cref="T:System.Object" />.</summary>
            public JournalPick(IEnumerable<JournalRecord<TValue>> RecordsAfter, IEnumerable<JournalRecord<TValue>> RecordsBefore)
            {
                this.RecordsAfter = RecordsAfter;
                this.RecordsBefore = RecordsBefore;
            }

            /// <summary>Последовательность записей после указанного времени (в порядке первый - ранний).</summary>
            public IEnumerable<JournalRecord<TValue>> RecordsAfter { get; private set; }

            /// <summary>Последовательность записей до указанного времени (в порядке первый - поздний).</summary>
            public IEnumerable<JournalRecord<TValue>> RecordsBefore { get; private set; }
        }
    }
}
