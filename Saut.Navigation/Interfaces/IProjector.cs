using System;

namespace Saut.Navigation.Interfaces
{
    /// <summary>Инструмент для преобразования глобальной координаты в локальную координату на маршруте</summary>
    public interface IProjector
    {
        /// <summary>Преобразует глобальную координату в локальную координату на маршруте</summary>
        /// <param name="Projection">Проекция маршрута</param>
        /// <param name="GlobalPosition">Глобальная координата</param>
        /// <returns>Локальная координата на маршруте</returns>
        Double GetRoutePosition(RouteProjection Projection, Double GlobalPosition);
    }
}
