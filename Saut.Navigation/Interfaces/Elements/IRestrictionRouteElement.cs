using System;

namespace Saut.Navigation.Interfaces.Elements
{
    /// <summary>Элемент маршрута, формирующий ограничение скорости</summary>
    public interface IRestrictionRouteElement : IRouteElement
    {
        /// <summary>Ограничение скорости</summary>
        Double SpeedRestriction { get; }

        /// <summary>Протяжённость ограничения скорости</summary>
        Double RestrictionLength { get; }
    }
}
