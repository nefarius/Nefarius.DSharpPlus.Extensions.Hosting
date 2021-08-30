using System;
using System.Collections.Generic;
using System.Linq;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Util
{
    internal static class AssemblyTypeHelper
    {
        public static IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit)
            where TAttribute : Attribute
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where t.IsDefined(typeof(TAttribute), inherit)
                select t;
        }
    }
}