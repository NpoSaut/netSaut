using System;
using System.Collections.Generic;
using System.Linq;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel
{
    /// <summary>Инструмент для линейной интерполяции на основе выборки из журнала.</summary>
    public class LinearInterpolator : IInterpolator<Double>
    {
        /// <summary>Путём интерполяции получает значение свойства в произвольный момент времени.</summary>
        /// <param name="Pick">Выборка из журнала в окрестности указанного времени.</param>
        /// <param name="Time">Время.</param>
        /// <returns>Значение свойства в указанное время, полученное путём интерполяции.</returns>
        public Double Interpolate(IJournalPick<Double> Pick, DateTime Time)
        {
            JournalRecord<Double>[] points = Zip(Pick).Take(2).ToArray();
            return points[0].Value + (Time.Ticks - points[0].Time.Ticks) * (points[1].Value - points[0].Value) / (points[1].Time.Ticks - points[0].Time.Ticks);
        }

        private IEnumerable<JournalRecord<Double>> Zip(IJournalPick<Double> Pick)
        {
            var enumerators = new[] { Pick.RecordsBefore.GetEnumerator(), Pick.RecordsAfter.GetEnumerator() };
            int i = 0;
            while (true)
            {
                if (enumerators[i].MoveNext()) yield return enumerators[i].Current;
                i = (i + 1) % 2;
            }
        }
    }
}
