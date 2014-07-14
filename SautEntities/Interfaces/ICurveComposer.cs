using System.Collections.Generic;

namespace Saut.Interfaces
{
    /// <summary>Композитор кривых</summary>
    /// <remarks>Реализует правила композиции множества кривых в единую, финальную</remarks>
    public interface ICurveComposer
    {
        /// <summary>Вычисляет композицию для указанных кривых</summary>
        /// <param name="Curves">Кривые для композиции</param>
        /// <returns>Кривая, учитывающая все указанные</returns>
        ICurve Compose(IEnumerable<ICurve> Curves);
    }
}
