using System;

namespace Saut.StateModel
{
    /// <summary>Свойство модели состояния.</summary>
    public interface IStateProperty
    {
        /// <summary>Название свойства.</summary>
        String Name { get; }

        /// <summary>Показывает, задано ли значение для свойства</summary>
        /// <returns>True, если значение задано</returns>
        Boolean HaveValue();
    }

    /// <summary>Свойство модели состояния.</summary>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    public interface IStateProperty<TValue> : IStateProperty
    {
        /// <summary>Получает текущее значение свойства.</summary>
        /// <returns>Значение свойства на момент запроса.</returns>
        TValue GetValue();

        /// <summary>Устанавливает новое значение свойства.</summary>
        /// <param name="NewValue">Новое значение свойства.</param>
        void UpdateValue(TValue NewValue);
    }

    /// <summary>Журналируемое свойство модели состояния.</summary>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    public interface IJournaledStateProperty<TValue> : IStateProperty<TValue>
    {
        /// <summary>Получает значение свойства в указанный момент времени.</summary>
        /// <param name="OnTime">Момент времени.</param>
        /// <returns>Значение свойства в указанный момент времени.</returns>
        TValue GetValue(DateTime OnTime);

        /// <summary>Показывает, задано ли значение для свойства в указанный момент времени</summary>
        /// <param name="OnTime">Момент времени</param>
        /// <returns>True, если значение задано</returns>
        Boolean HaveValue(DateTime OnTime);

        /// <summary>Устанавливает новое значение свойства.</summary>
        /// <param name="NewValue">Новое значение свойства.</param>
        /// <param name="OnTime">Момент актуализации значения.</param>
        void UpdateValue(TValue NewValue, DateTime OnTime);
    }
}
