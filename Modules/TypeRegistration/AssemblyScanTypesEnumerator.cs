using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Modules.TypeRegistration
{
    public class AssemblyScanTypesEnumerator : ITypesEnumerator
    {
        private readonly Assembly _assembly;
        private readonly Type _baseType;

        public AssemblyScanTypesEnumerator(Assembly Assembly, Type BaseType)
        {
            _assembly = Assembly;
            _baseType = BaseType;
        }

        public IEnumerable<Type> EnumerateTypes() { return _assembly.GetTypes().Where(t => _baseType.IsAssignableFrom(t)); }
    }
}
