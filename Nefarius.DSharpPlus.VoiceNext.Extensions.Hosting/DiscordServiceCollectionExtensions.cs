using System;
using DSharpPlus.VoiceNext;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.VoiceNext.Extensions.Hosting
{
    [UsedImplicitly]
    public static class DiscordServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds VoiceNext extension to <see cref="IDiscordClientService"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">The <see cref="DiscordVoiceNextOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddDiscordVoiceNext(
            this IServiceCollection services,
            Action<DiscordVoiceNextOptions> configure
        )
        {
            services.AddSingleton(typeof(IDiscordExtensionConfiguration), provider =>
            {
                var options = new DiscordVoiceNextOptions();

                configure(options);

                var discord = provider.GetRequiredService<IDiscordClientService>().Client;

                discord.UseVoiceNext(options.Configuration);

                return options;
            });

            return services;
        }
    }
}