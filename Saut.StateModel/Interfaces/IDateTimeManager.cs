using System;

namespace Saut.StateModel.Interfaces
{
    /// <summary>Объект по работе с часами реального времени.</summary>
    public interface IDateTimeManager
    {
        /// <summary>Возвращает текущее время.</summary>
        DateTime Now { get; }
    }
}
