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
        ///     Adds VoiceNext extension to <see cref="IDiscordClientService" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configure">The <see cref="VoiceNextConfiguration" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddDiscordVoiceNext(
            this IServiceCollection services,
            Action<VoiceNextConfiguration?> configure = null
        )
        {
            services.AddSingleton(typeof(IDiscordExtensionConfiguration), provider =>
            {
                var options = new VoiceNextConfiguration();

                configure?.Invoke(options);

                var discord = provider.GetRequiredService<IDiscordClientService>().Client;

                discord.UseVoiceNext(options);

                //
                // This is intentional; we don't need this "service", just the execution flow ;)
                // 
                return null;
            });

            return services;
        }
    }
}