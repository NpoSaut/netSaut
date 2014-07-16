using System;
using System.Runtime.Serialization;

namespace Modules.Boot
{
    /// <Summary>Исключения процесса загрузки</Summary>
    [Serializable]
    public abstract class BootstrapperException : ApplicationException
    {
        protected BootstrapperException() : base("Исключения процесса загрузки") { }
        protected BootstrapperException(Exception inner) : base("Исключения процесса загрузки", inner) { }
        protected BootstrapperException(string message) : base(message) { }
        protected BootstrapperException(string message, Exception inner) : base(message, inner) { }

        protected BootstrapperException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
