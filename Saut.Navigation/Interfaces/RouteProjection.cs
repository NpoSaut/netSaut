using System;

namespace Saut.Navigation.Interfaces
{
    /// <summary>Проекция маршрута на глобальную координату</summary>
    public class RouteProjection
    {
        public RouteProjection(IRoute Route, double StartPoint)
        {
            this.Route = Route;
            this.StartPoint = StartPoint;
        }

        /// <summary>Маршрут</summary>
        public IRoute Route { get; private set; }

        /// <summary>Глобальная координата начала маршрута</summary>
        public Double StartPoint { get; private set; }
    }
}
