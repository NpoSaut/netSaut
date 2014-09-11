using System;
using System.Linq;
using Saut.StateModel.Exceptions;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Interpolators
{
    /// <summary>Ступенчатый интерполятор.</summary>
    /// <remarks>Возвращает последнее значение, установленное до указанного момента.</remarks>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    public class StepInterpolator<TValue> : IInterpolator<TValue>
    {
        /// <summary>Путём интерполяции получает значение свойства в произвольный момент времени.</summary>
        /// <param name="Pick">Выборка из журнала в окрестности указанного времени.</param>
        /// <param name="Time">Время.</param>
        /// <returns>Значение свойства в указанное время, полученное путём интерполяции.</returns>
        public TValue Interpolate(IJournalPick<TValue> Pick, DateTime Time)
        {
            JournalRecord<TValue>[] recs = Pick.RecordsBefore.Take(1).ToArray();
            if (recs.Length == 0) throw new PropertyValueUndefinedException();
            return recs[0].Value;
        }

        /// <summary>Проверяет, может ли свойство быть интерполировано в заданной окрестности</summary>
        /// <param name="Pick">Выборка из журнала в окрестности указанного времени</param>
        /// <param name="Time">Время</param>
        /// <returns>True, если свойство может быть интерполировано в заданной окрестности</returns>
        public bool CanInterpolate(IJournalPick<TValue> Pick, DateTime Time) { return Pick.RecordsBefore.Any(); }
    }
}
