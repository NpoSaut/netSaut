using Saut.Entities;

namespace Saut.Interfaces
{
    /// <summary>Рассчитывает кривую подхода к цели с ограничением</summary>
    public interface ICurveCalculator
    {
        /// <summary>Рассчитывает кривую подхода к цели с ограничением</summary>
        /// <param name="Target">Ограничение</param>
        /// <returns>Кривая подхода к ограничению</returns>
        ICurve CalculateCurve(RestrictionTarget Target);
    }
}
