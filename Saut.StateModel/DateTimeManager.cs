using System;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel
{
    /// <summary>Менеджер даты-времени, привязанный к текущей дате через статические свойства объекта DateTime.</summary>
    public class DateTimeManager : IDateTimeManager
    {
        /// <summary>Возвращает текущее время.</summary>
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}
