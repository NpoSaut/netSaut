using System;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Obsoleting
{
    /// <summary>Политика проверка журнальной выборки на актуальность значений</summary>
    public interface IObsoletePolicy
    {
        /// <summary>Декорирует журнальную выборку таким образом, чтобы в ней оставались только актуальные значения</summary>
        /// <param name="Pick">Исходная выборка</param>
        /// <param name="Time">Время</param>
        /// <returns>Журнальная выборка, содержащая только актуальные значения</returns>
        IJournalPick<TValue> DecoratePick<TValue>(IJournalPick<TValue> Pick, DateTime Time);
    }
}
