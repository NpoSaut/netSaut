using System.Collections.Generic;
using Saut.Entities;

namespace Saut.Interfaces
{
    /// <summary>Источник кривых</summary>
    public interface ICurveProvider
    {
        /// <summary>Род кривых из этого источника</summary>
        RestrictionKind Kind { get; }

        /// <summary>Перечисляет все кривые из этого источника</summary>
        IEnumerable<ICurve> GetCurves();
    }
}
