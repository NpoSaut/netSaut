using System.Collections.Generic;
using System.Linq;
using Saut.Navigation.Entities;
using Saut.Navigation.Interfaces;

namespace Saut.Navigation
{
    /// <summary>Навигатор по маршруту</summary>
    public class RouteNavigator : INavigator
    {
        private readonly IProjector _projector;
        private readonly RouteProjection _routeProjection;

        public RouteNavigator(RouteProjection RouteProjection, IProjector Projector)
        {
            _projector = Projector;
            _routeProjection = RouteProjection;
        }

        /// <summary>Получает список ближайших целей по маршруту для указанной координаты</summary>
        /// <param name="MyPosition">Текущая позиция</param>
        /// <returns>Список целей по указанному маршруту</returns>
        public IEnumerable<RouteTarget> GetTargets(double MyPosition)
        {
            double routePosition = _projector.GetRoutePosition(_routeProjection, MyPosition);
            IRoute route = _routeProjection.Route;
            return route.Elements.Select(e => new RouteTarget(e.Element, e.Position - routePosition)).SkipWhile(t => t.Disstance < 0);
        }
    }
}
