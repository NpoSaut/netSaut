using System;
using System.Collections.Generic;
using Geographics;

namespace Saut.StateModel.Interpolators.InterpolationTools
{
    /// <summary>Инструмент взвешенных вычислений для gps-координат</summary>
    public class EarthPointWeightingTool : VectorWeightingToolBase<EarthPoint, Double>
    {
        public EarthPointWeightingTool(IWeightingTool<double> NumericWeightingTool) : base(NumericWeightingTool) { }

        /// <summary>Разбивает сложную величину <paramref name="Value" /> на численные составляющие</summary>
        /// <param name="Value">Векторизируемая величина</param>
        /// <returns>Величина, разбитая на численные составляющие</returns>
        protected override IList<double> Vectorize(EarthPoint Value) { return new[] { Value.Latitude.Value, Value.Longitude.Value }; }

        /// <summary>Собирает сложную величину из численных составляющих</summary>
        /// <param name="Vector">Векторизированная величина</param>
        /// <returns>Восстановленная сложная величина</returns>
        protected override EarthPoint Devectorize(IList<double> Vector) { return new EarthPoint(Vector[0], Vector[1]); }
    }
}
