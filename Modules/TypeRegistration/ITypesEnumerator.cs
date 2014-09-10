using System;
using System.Collections.Generic;

namespace Modules.TypeRegistration
{
    public interface ITypesEnumerator
    {
        IEnumerable<Type> EnumerateTypes(); 
    }
}