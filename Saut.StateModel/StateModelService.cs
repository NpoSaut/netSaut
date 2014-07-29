using System.Collections.Generic;
using System.Linq;

namespace Saut.StateModel
{
    public class StateModelService : IStateModelService
    {
        private readonly IStateProperty[] _properties;
        public StateModelService(IStateProperty[] Properties) { _properties = Properties; }

        public IEnumerable<TProperty> GetProperties<TProperty>() where TProperty : IStateProperty { return _properties.OfType<TProperty>(); }
    }
}
