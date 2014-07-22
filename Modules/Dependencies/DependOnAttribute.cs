using System;
using System.Collections.Generic;

namespace Modules.Dependencies
{
    /// <summary>Атрибут зависимости модуля от сервисов</summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependOnAttribute : Attribute
    {
        /// <summary>Устанавливает зависимости от указанных сервисов</summary>
        /// <param name="Dependencies">Необходимые для работы сервисы</param>
        public DependOnAttribute(params Type[] Dependencies) { this.Dependencies = Dependencies; }

        /// <summary>Необходимые для работы сервисы</summary>
        public IList<Type> Dependencies { get; private set; }
    }
}
