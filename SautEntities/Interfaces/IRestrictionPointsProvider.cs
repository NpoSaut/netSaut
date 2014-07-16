using System.Collections.Generic;
using Saut.Entities;

namespace Saut.Interfaces
{
    /// <summary>Источник информации о впереди лежащих ограничениях</summary>
    public interface IRestrictionPointsProvider
    {
        /// <summary>Возвращает последовательность впереди лежащих ограничений</summary>
        IEnumerable<RestrictionTarget> GetRestrictionPoints();
    }
}
