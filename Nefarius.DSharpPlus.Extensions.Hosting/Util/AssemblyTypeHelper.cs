using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting")]
[assembly: InternalsVisibleTo("Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting")]
[assembly: InternalsVisibleTo("Nefarius.DSharpPlus.Interactivity.Extensions.Hosting")]
[assembly: InternalsVisibleTo("Nefarius.DSharpPlus.VoiceNext.Extensions.Hosting")]

namespace Nefarius.DSharpPlus.Extensions.Hosting.Util;

internal static class AssemblyTypeHelper
{
    public static IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit = true)
        where TAttribute : Attribute
    {
        return from a in AppDomain.CurrentDomain.GetAssemblies()
            from t in a.GetTypes()
            where t.IsDefined(typeof(TAttribute), inherit)
            select t;
    }
}