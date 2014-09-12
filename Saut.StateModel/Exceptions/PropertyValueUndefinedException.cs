using System;
using System.Runtime.Serialization;

namespace Saut.StateModel.Exceptions
{
    /// <Summary>Значение свойства не было определено или устарело</Summary>
    [Serializable]
    public class PropertyValueUndefinedException : StateModelException
    {
        public PropertyValueUndefinedException() : base("Значение свойства не было определено или устарело или устарело") { }
        public PropertyValueUndefinedException(Exception inner) : base("Значение свойства не было определено или устарело", inner) { }
        public PropertyValueUndefinedException(string message) : base(message) { }
        public PropertyValueUndefinedException(string message, Exception inner) : base(message, inner) { }

        public PropertyValueUndefinedException(IStateProperty Property, DateTime OnTime)
            : base(String.Format("Значение свойства {0} не было определено или устарело в момент {1:mm:ss.fff}", Property, OnTime)) { }

        protected PropertyValueUndefinedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
