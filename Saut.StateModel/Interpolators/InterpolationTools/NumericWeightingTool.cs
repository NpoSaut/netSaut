using System;

namespace Saut.StateModel.Interpolators.InterpolationTools
{
    /// <summary>Инструмент взвешенных вычислений для дробных чисел</summary>
    public class NumericWeightingTool : IWeightingTool<Double>
    {
        /// <summary>Получает среднее арифметическое взвешенное двух указанных величин</summary>
        /// <param name="ValueA">Величина A</param>
        /// <param name="ValueB">Величина B</param>
        /// <param name="ValueBWeight">Вес величины B (позиция на отрезце A-B)</param>
        /// <returns>Среднее арифметическое взвешенное величин A и B</returns>
        public double GetWeightedArithmeticMean(double ValueA, double ValueB, double ValueBWeight)
        {
            return ValueA * (1 - ValueBWeight) + ValueB * ValueBWeight;
        }
    }
}
