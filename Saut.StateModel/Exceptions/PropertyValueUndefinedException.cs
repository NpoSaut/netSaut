using System;
using System.Runtime.Serialization;

namespace Saut.StateModel.Exceptions
{
    /// <Summary>Значение свойства не было определено</Summary>
    [Serializable]
    public class PropertyValueUndefinedException : StateModelException
    {
        public PropertyValueUndefinedException() : base("Значение свойства не было определено") { }
        public PropertyValueUndefinedException(Exception inner) : base("Значение свойства не было определено", inner) { }
        public PropertyValueUndefinedException(string message) : base(message) { }
        public PropertyValueUndefinedException(string message, Exception inner) : base(message, inner) { }

        protected PropertyValueUndefinedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
