namespace Saut.Interfaces
{
    /// <summary>Модификатор кривых торможения</summary>
    public interface ICurveModifier
    {
        /// <summary>Модифицирует кривую</summary>
        /// <param name="Curve">Исходная кривая</param>
        /// <returns>Модифицированная кривая</returns>
        ICurve Modify(ICurve Curve);
    }
}
