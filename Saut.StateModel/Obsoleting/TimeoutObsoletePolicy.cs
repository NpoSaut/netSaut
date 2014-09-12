using System;
using Saut.StateModel.Interfaces;

namespace Saut.StateModel.Obsoleting
{
    /// <summary>Политика проверки актуальности значений на основании времени их получения</summary>
    public class TimeoutObsoletePolicy : IObsoletePolicy
    {
        private readonly TimeSpan _obsoleteTimeout;

        /// <summary>Создаёт политику проверки актуальности значений на основании времени их получения</summary>
        /// <param name="ObsoleteTimeout">Время, в течении которого значение свойства считается актуальным</param>
        public TimeoutObsoletePolicy(TimeSpan ObsoleteTimeout) { _obsoleteTimeout = ObsoleteTimeout; }

        /// <summary>Время устаревания свойства</summary>
        public TimeSpan ObsoleteTimeout
        {
            get { return _obsoleteTimeout; }
        }

        /// <summary>Декорирует журнальную выборку таким образом, чтобы в ней оставались только актуальные значения</summary>
        /// <param name="Pick">Исходная выборка</param>
        /// <param name="Time">Время</param>
        /// <returns>Журнальная выборка, содержащая только актуальные значения</returns>
        public IJournalPick<TValue> DecoratePick<TValue>(IJournalPick<TValue> Pick, DateTime Time)
        {
            return new PredicateJournalPickDecorator<TValue>(Pick,
                                                             r => r.Time <= Time + ObsoleteTimeout,
                                                             r => r.Time >= Time - ObsoleteTimeout);
        }
    }
}
