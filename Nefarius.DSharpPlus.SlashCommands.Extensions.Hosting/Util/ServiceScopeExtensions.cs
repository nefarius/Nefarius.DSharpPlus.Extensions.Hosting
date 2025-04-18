using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.Events;

namespace Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.Util;

internal static class ServiceScopeExtensions
{
    public static IList<IDiscordSlashCommandsEventsSubscriber> GetDiscordSlashCommandsEventsSubscriber(
        this IServiceScope scope
    )
    {
        return scope.ServiceProvider
            .GetServices(typeof(IDiscordSlashCommandsEventsSubscriber))
            .Cast<IDiscordSlashCommandsEventsSubscriber>()
            .ToList();
    }
}