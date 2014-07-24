using System;
using System.Collections.Generic;

namespace Modules.Dependencies
{
    /// <summary>Атрибут предоставления модулем сервисов</summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ProvidesAttribute : Attribute
    {
        /// <summary>Устанавливает сервисы, предоставляемые модулем</summary>
        /// <param name="ProvidedServices">Список предоставляемых сервисов</param>
        public ProvidesAttribute(params Type[] ProvidedServices) { this.ProvidedServices = ProvidedServices; }

        /// <summary>Предоставляемые сервисы</summary>
        public IList<Type> ProvidedServices { get; private set; }
    }
}
