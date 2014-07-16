using System;
using System.Collections.Generic;
using Saut.Navigation.Interfaces.Targeting;

namespace Saut.Navigation.Interfaces
{
    /// <summary>Инструмент для получения списка ближайших целей исходя из текущей координаты</summary>
    public interface INavigator
    {
        /// <summary>Получает список ближайших целей для указанной координаты</summary>
        /// <param name="MyPosition">Текущая позиция</param>
        /// <returns>Список целей по указанному маршруту</returns>
        IEnumerable<RouteTarget> GetTargets(Double MyPosition);
    }
}
