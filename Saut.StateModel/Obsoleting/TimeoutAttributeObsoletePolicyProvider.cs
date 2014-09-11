using System;
using System.Reflection;

namespace Saut.StateModel.Obsoleting
{
    /// <summary>Генератор политик устаревания для свойства, основывающийся на таймауте устаревания, определяемым атрибутом
    ///     <see cref="ObsoleteTimeoutAttribute" /> и значением по-умолчанию</summary>
    public class TimeoutAttributeObsoletePolicyProvider : IObsoletePolicyProvider
    {
        public TimeoutAttributeObsoletePolicyProvider() { DefaultObsoleteTimeout = TimeSpan.FromSeconds(2); }

        /// <summary>Время устаревания свойства по-умолчанию</summary>
        public TimeSpan DefaultObsoleteTimeout { get; set; }

        /// <summary>Получает политику устаревания свойства</summary>
        /// <param name="Property">Свойство, для которого требуется получить политику устаревания</param>
        /// <returns>Политика устаревания для указанного свойства</returns>
        public IObsoletePolicy GetObsoletePolicy(IStateProperty Property)
        {
            var attribute = Property.GetType().GetCustomAttribute<ObsoleteTimeoutAttribute>(true);
            TimeSpan timeout = attribute != null ? attribute.Timeout : DefaultObsoleteTimeout;
            return new TimeoutObsoletePolicy(timeout);
        }
    }
}
