using System;

namespace Saut.StateModel.Interpolators.InterpolationTools
{
    /// <summary>Инструмент взвешенных вычислений</summary>
    /// <typeparam name="TValue">Тип величины</typeparam>
    public interface IWeightingTool<TValue>
    {
        /// <summary>Получает среднее арифметическое взвешенное двух указанных величин</summary>
        /// <param name="ValueA">Величина A</param>
        /// <param name="ValueB">Величина B</param>
        /// <param name="ValueBWeight">Вес величины A</param>
        /// <returns>Среднее арифметическое взвешенное величин A и B</returns>
        TValue GetWeightedArithmeticMean(TValue ValueA, TValue ValueB, Double ValueBWeight);
    }
}
