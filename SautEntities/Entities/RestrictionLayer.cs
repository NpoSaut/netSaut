using System.Collections.Generic;
using System.Linq;
using Saut.Interfaces;

namespace Saut.Entities
{
    /// <summary>Тип слоя ограничений</summary>
    public enum RestrictionKind
    {
        /// <summary>Связанные с конструкционными особенностями подвижной единицы</summary>
        Construct,

        /// <summary>Глобальные ограничения по Дороге</summary>
        Global,

        /// <summary>Местные ограничения</summary>
        /// <remarks>
        ///     Ограничения, имеющие своё местоположение и протяжённость. Например, связанные с кривизной участка,
        ///     проследование тоннеля и т.п.
        /// </remarks>
        Local,

        /// <summary>От последнего проследованного светофора</summary>
        CurrentTrafficLight,

        /// <summary>От впереди стоящих светофоров</summary>
        ComingTrafficLights
    }

    /// <summary>Слой ограничений</summary>
    /// <remarks>Слой объединяет кривые ограничений от источников одного рода</remarks>
    public class RestrictionLayer
    {
        public RestrictionLayer(RestrictionKind Kind, IEnumerable<ICurve> Curves)
        {
            this.Kind = Kind;
            this.Curves = Curves.ToList();
        }

        /// <summary>Тип ограничений на слое</summary>
        public RestrictionKind Kind { get; private set; }

        /// <summary>Кривые ограничений</summary>
        public IList<ICurve> Curves { get; private set; }
    }
}
