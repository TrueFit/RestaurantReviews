using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NoREST.Tests.Utilities
{
    public static class ReflectionTricks
    {
        public static IEnumerable<Type> GetAllTypesOfType(Type typeWithinDesiredAssembly, Type targetType)
        {
            return Assembly.GetAssembly(typeWithinDesiredAssembly)
                .GetTypes()
                .Where(t => t.IsSubclassOf(targetType));
        }
    }
}
