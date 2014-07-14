using System.Collections.Generic;
using Saut.Entities;

namespace Saut.Interfaces
{
    public interface ICurveAggregator
    {
        IEnumerable<RestrictionLayer> Aggregate(IEnumerable<ICurveProvider> CurveProviders);
    }
}