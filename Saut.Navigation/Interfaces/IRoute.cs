﻿using System.Collections.Generic;
using Saut.Navigation.Entities;

namespace Saut.Navigation.Interfaces
{
    /// <summary>Маршрут</summary>
    public interface IRoute
    {
        /// <summary>Элементы маршрута, расположенные в порядке от начала к концу маршрута</summary>
        IEnumerable<RoutePoint> Elements { get; }
    }
}
