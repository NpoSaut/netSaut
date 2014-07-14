using System;
using System.Collections.Generic;

namespace Saut.Interfaces
{
    /// <summary>Рельеф</summary>
    public interface IRelief
    {
        /// <summary>Возвращает значение уклона в указанной точке</summary>
        /// <remarks>Возвращаемое значение должно быть рассчитано относительно следующей точки (как производная функции)</remarks>
        /// <param name="Disstance">Расстояние до точки</param>
        /// <returns>Уклон на указанном расстоянии</returns>
        Double Slope(Double Disstance);
    }

    /// <summary>Сегментированный рельеф</summary>
    /// <remarks>Рельеф, построенный через массив прямолинейных сегментов</remarks>
    public interface ISegmentedRelief : IRelief
    {
        /// <summary>Расстояния до ближайших точек излома кривой рельефа</summary>
        IEnumerable<Double> BreakPoints { get; }
    }
}
