using System;
using Saut.Navigation.Interfaces.Elements;

namespace Saut.Navigation.Entities
{
    /// <summary>Объект, расположенный на маршруте</summary>
    public class RoutePoint
    {
        /// <summary>Положение объекта на маршруте</summary>
        public Double Position { get; private set; }

        /// <summary>Объект маршрута</summary>
        public IRouteElement Element { get; private set; }
    }
}
