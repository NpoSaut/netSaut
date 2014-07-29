using System;

namespace Saut.StateModel.Interfaces
{
    /// <summary>
    /// Инструмент по созданию выборок из журнала
    /// </summary>
    public interface IRecordPicker
    {
        /// <summary>Создаёт выборку из журнала в окрестности указанного времени.</summary>
        /// <typeparam name="TValue">Тип значений в журнале.</typeparam>
        /// <param name="Journal">Журнал, в котором будет выполняться поиск.</param>
        /// <param name="Time">Время, в окрестности которого нужно сделать выборку.</param>
        /// <returns>Выборку из журнала в окрестности указанного времени.</returns>
        IJournalPick<TValue> PickRecords<TValue>(IJournal<TValue> Journal, DateTime Time);
    }
}
