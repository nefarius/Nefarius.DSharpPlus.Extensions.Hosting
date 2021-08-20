using System;
using DSharpPlus;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    [UsedImplicitly]
    public static class DiscordServiceCollectionExtensions
    {
        [UsedImplicitly]
        public static IServiceCollection AddDiscord(
            this IServiceCollection services,
            Action<DiscordConfiguration> configure = null
        )
        {
            if (configure != null)
            {
                services.Configure(configure);
            }

            services.TryAddSingleton<IDiscordClientService, DiscordService>();

            return services;
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordHostedService(
            this IServiceCollection services
        )
        {
            return services.AddHostedService<DiscordHostedService>();
        }
    }
}
