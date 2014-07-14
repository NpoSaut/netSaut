using Saut.Entities;

namespace Saut.Interfaces
{
    /// <summary>Сервис модификаторов</summary>
    public interface IModifiersService
    {
        /// <summary>Модифицирует кривую с учётом рода её ограничений</summary>
        /// <param name="LayerKind">Род ограничений кривой</param>
        /// <param name="OriginalCurve">Оригинальная кривая</param>
        /// <returns>Модифицированная кривая</returns>
        ICurve ModifyLayerCurve(RestrictionKind LayerKind, ICurve OriginalCurve);
    }
}
