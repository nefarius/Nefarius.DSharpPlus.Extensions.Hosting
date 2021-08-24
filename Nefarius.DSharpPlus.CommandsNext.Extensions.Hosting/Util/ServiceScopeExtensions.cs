using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Events;

namespace Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Util
{
    internal static class ServiceScopeExtensions
    {
        public static IList<IDiscordCommandsNextEventsSubscriber> GetDiscordCommandsNextEventsSubscriber(
            this IServiceScope scope
        )
        {
            return scope.ServiceProvider
                .GetServices(typeof(IDiscordCommandsNextEventsSubscriber))
                .Cast<IDiscordCommandsNextEventsSubscriber>()
                .ToList();
        }
    }
}