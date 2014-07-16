using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Modules.Dependencies
{
    /// <Summary>Невозможно разрешить зависимости между модулями</Summary>
    [Serializable]
    public class UnresolvableDependenciesException : BootstrapperException
    {
        protected UnresolvableDependenciesException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }

        public UnresolvableDependenciesException(IList<IModule> UninitializedModules, List<Type> InitializedTypes)
        {
            this.UninitializedModules = UninitializedModules;
            this.InitializedTypes = InitializedTypes;
        }

        public IList<IModule> UninitializedModules { get; set; }
        public List<Type> InitializedTypes { get; set; }

        /// <summary>Возвращает сообщение, описывающее текущее исключение.</summary>
        /// <returns>Сообщение об ошибке с объяснением причин исключения или пустая строка ("").</returns>
        public override string Message
        {
            get
            {
                return string.Format("Невозможно разрешить зависимости для модулей:\n {0}\nБыли инициализированы сервисы:\n{1}",
                                     string.Join("\n", UninitializedModules.Select(m => string.Format(" - {0}", m))),
                                     string.Join("\n", InitializedTypes.Select(s => string.Format(" - {0}", s.FullName))));
            }
        }
    }
}
