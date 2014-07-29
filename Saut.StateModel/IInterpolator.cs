using System;

namespace Saut.StateModel
{
    /// <summary>Инструмент по интерполяции кривой из журнала.</summary>
    /// <typeparam name="TValue">Тип значений в журнале.</typeparam>
    public interface IInterpolator<TValue>
    {
        /// <summary>Путём интерполяции получает значение свойства в произвольный момент времени.</summary>
        /// <param name="Journal">Журнал свойства.</param>
        /// <param name="Time">Время.</param>
        /// <returns>Значение свойства в указанное время, полученное путём интерполяции.</returns>
        TValue Interpolate(IJournal<TValue> Journal, DateTime Time);
    }
}
