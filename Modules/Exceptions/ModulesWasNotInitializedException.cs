using System;
using System.Runtime.Serialization;

namespace Modules.Exceptions
{
    /// <Summary>Модули ещё не были инициализированы</Summary>
    [Serializable]
    public class ModulesWasNotInitializedException : BootstrapperException
    {
        public ModulesWasNotInitializedException() : base("Модули ещё не были инициализированы") { }
        public ModulesWasNotInitializedException(Exception inner) : base("Модули ещё не были инициализированы", inner) { }
        public ModulesWasNotInitializedException(string message) : base(message) { }
        public ModulesWasNotInitializedException(string message, Exception inner) : base(message, inner) { }

        protected ModulesWasNotInitializedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
