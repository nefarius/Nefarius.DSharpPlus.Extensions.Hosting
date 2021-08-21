using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.Extensions.Hosting.Events.CommandsNext;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Util
{
    internal static partial class ServiceScopeExtensions
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
