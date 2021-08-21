using System;
using DSharpPlus.Interactivity.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.Interactivity.Extensions.Hosting
{
    [UsedImplicitly]
    public static class DiscordServiceCollectionExtensions
    {
        /// <summary>
        ///     Registers the <see cref="DiscordInteractivityOptions"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">The <see cref="DiscordInteractivityOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddDiscordInteractivity(
            this IServiceCollection services,
            Action<DiscordInteractivityOptions> configure
        )
        {
            services.AddSingleton(typeof(IDiscordExtensionConfiguration), provider =>
            {
                var options = new DiscordInteractivityOptions();

                configure(options);

                var discord = provider.GetRequiredService<IDiscordClientService>().Client;

                discord.UseInteractivity(options.Configuration);

                return options;
            });

            return services;
        }
    }
}