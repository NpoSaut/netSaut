using System;

namespace Saut.StateModel.Obsoleting
{
    /// <summary>Атрибут, устанавливающий время устаревания значения свойства</summary>
    public class ObsoleteTimeoutAttribute : Attribute
    {
        /// <summary>Атрибут, устанавливающий время устаревания значения свойства</summary>
        /// <param name="TimeoutMs">Таймаут устаревания значения свойства в миллисекундах</param>
        public ObsoleteTimeoutAttribute(int TimeoutMs) : this(TimeSpan.FromMilliseconds(TimeoutMs)) { }

        /// <summary>Атрибут, устанавливающий время устаревания значения свойства</summary>
        /// <param name="Timeout">Таймаут устаревания значения свойства</param>
        public ObsoleteTimeoutAttribute(TimeSpan Timeout) { this.Timeout = Timeout; }

        /// <summary>Таймаут устаревания значения свойства</summary>
        public TimeSpan Timeout { get; private set; }
    }
}
