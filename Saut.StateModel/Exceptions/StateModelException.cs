using System;
using System.Runtime.Serialization;

namespace Saut.StateModel.Exceptions
{
    /// <Summary>Исключение при работе с моделью свойств</Summary>
    [Serializable]
    public abstract class StateModelException : ApplicationException
    {
        protected StateModelException(Exception inner) : base("Исключение при работе с моделью свойств", inner) { }
        protected StateModelException(string message) : base(message) { }
        protected StateModelException(string message, Exception inner) : base(message, inner) { }

        protected StateModelException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
