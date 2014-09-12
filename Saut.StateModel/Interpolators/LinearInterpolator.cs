using System;
using System.Collections.Generic;
using System.Linq;
using Saut.StateModel.Exceptions;
using Saut.StateModel.Interfaces;
using Saut.StateModel.Interpolators.InterpolationTools;

namespace Saut.StateModel.Interpolators
{
    /// <summary>Инструмент для линейной интерполяции на основе выборки из журнала.</summary>
    /// <remarks>
    ///     Выполняет линейную интерполяцию значения на основе значения до и после указанного времени. Если указанное
    ///     время больше, чем последняя запись в журнале, производит линейную экстраполяцию на основе последних двух точек до
    ///     указанного момента.
    /// </remarks>
    public class LinearInterpolator<TValue> : IInterpolator<TValue>
    {
        private readonly IWeightingTool<TValue> _weightingTool;
        public LinearInterpolator(IWeightingTool<TValue> WeightingTool) { _weightingTool = WeightingTool; }

        /// <summary>Путём интерполяции получает значение свойства в произвольный момент времени.</summary>
        /// <param name="Pick">Выборка из журнала в окрестности указанного времени.</param>
        /// <param name="Time">Время.</param>
        /// <returns>Значение свойства в указанное время, полученное путём интерполяции.</returns>
        public TValue Interpolate(IJournalPick<TValue> Pick, DateTime Time)
        {
            JournalRecord<TValue>[] points = Zip(Pick).Take(2).ToArray();
            if (points.Length < 2) throw new PropertyValueUndefinedException();
            double weight = ((Double)(Time.Ticks - points[0].Time.Ticks)) / (points[1].Time.Ticks - points[0].Time.Ticks);
            return _weightingTool.GetWeightedArithmeticMean(points[0].Value, points[1].Value, weight);
        }

        /// <summary>Проверяет, может ли свойство быть интерполировано в заданной окрестности</summary>
        /// <param name="Pick">Выборка из журнала в окрестности указанного времени</param>
        /// <param name="Time">Время</param>
        /// <returns>True, если свойство может быть интерполировано в заданной окрестности</returns>
        public bool CanInterpolate(IJournalPick<TValue> Pick, DateTime Time)
        {
            JournalRecord<TValue>[] points = Zip(Pick).Take(2).ToArray();
            return points.Length >= 2;
        }

        private static IEnumerable<JournalRecord<TValue>> Zip(IJournalPick<TValue> Pick)
        {
            var enumerators = new[] { Pick.RecordsBefore.GetEnumerator(), Pick.RecordsAfter.GetEnumerator() };
            var hasElements = new[] { true, true };
            int i = 0;
            while (hasElements.Any(e => e))
            {
                if (enumerators[i].MoveNext()) yield return enumerators[i].Current;
                else hasElements[i] = false;
                i = (i + 1) % 2;
            }
        }
    }
}
