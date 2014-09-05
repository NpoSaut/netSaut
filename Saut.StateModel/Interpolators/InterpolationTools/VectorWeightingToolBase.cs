using System;
using System.Collections.Generic;
using System.Linq;

namespace Saut.StateModel.Interpolators.InterpolationTools
{
    /// <summary>Инструмент взвешенных вычислений для векторизируемых величин</summary>
    /// <typeparam name="TComplexValue">Тип комплексной величины</typeparam>
    /// <typeparam name="TComponentValue"></typeparam>
    public abstract class VectorWeightingToolBase<TComplexValue, TComponentValue> : IWeightingTool<TComplexValue>
    {
        private readonly IWeightingTool<TComponentValue> _numericWeightingTool;
        public VectorWeightingToolBase(IWeightingTool<TComponentValue> NumericWeightingTool) { _numericWeightingTool = NumericWeightingTool; }

        /// <summary>Получает среднее арифметическое взвешенное двух указанных величин</summary>
        /// <param name="ValueA">Величина A</param>
        /// <param name="ValueB">Величина B</param>
        /// <param name="ValueBWeight">Вес величины A</param>
        /// <returns>Среднее арифметическое взвешенное величин A и B</returns>
        public TComplexValue GetWeightedArithmeticMean(TComplexValue ValueA, TComplexValue ValueB, double ValueBWeight)
        {
            IList<TComponentValue> vectorA = Vectorize(ValueA);
            IList<TComponentValue> vectorB = Vectorize(ValueB);
            List<TComponentValue> meanVector =
                Enumerable.Range(0, Math.Min(vectorA.Count, vectorB.Count))
                          .Select(i => _numericWeightingTool.GetWeightedArithmeticMean(vectorA[i], vectorB[i], ValueBWeight))
                          .ToList();
            return Devectorize(meanVector);
        }

        /// <summary>Разбивает сложную величину <paramref name="Value" /> на численные составляющие</summary>
        /// <param name="Value">Векторизируемая величина</param>
        /// <returns>Величина, разбитая на численные составляющие</returns>
        protected abstract IList<TComponentValue> Vectorize(TComplexValue Value);

        /// <summary>Собирает сложную величину из численных составляющих</summary>
        /// <param name="Vector">Векторизированная величина</param>
        /// <returns>Восстановленная сложная величина</returns>
        protected abstract TComplexValue Devectorize(IList<TComponentValue> Vector);
    }
}
