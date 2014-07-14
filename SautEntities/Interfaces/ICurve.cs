using System;

namespace Saut.Interfaces
{
    /// <summary>Кривая торможения</summary>
    public interface ICurve
    {
        /// <summary>Расстояние до точки прицельной остановки</summary>
        Double StopThrough { get; }

        /// <summary>Зависимость ограничения скорости от расстояния</summary>
        /// <param name="Disstance">Расстояние до точки</param>
        /// <returns>Допустимая скорость в указанной точке</returns>
        Double GetSpeed(Double Disstance);
    }
}
