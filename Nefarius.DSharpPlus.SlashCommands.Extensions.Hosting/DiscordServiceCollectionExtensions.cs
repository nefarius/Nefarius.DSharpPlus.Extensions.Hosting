using System;
using DSharpPlus.SlashCommands;
using Emzi0767;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting
{
    [UsedImplicitly]
    public static class DiscordServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds Interactivity extension to <see cref="IDiscordClientService" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="SlashCommandsConfiguration" />.</param>
        /// <param name="extension"></param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddDiscordSlashCommands(
            this IServiceCollection services,
            Action<SlashCommandsConfiguration?> configuration = null,
            Action<SlashCommandsExtension?> extension = null
        )
        {
            services.AddSingleton(typeof(IDiscordExtensionConfiguration), provider =>
            {
                var options = new SlashCommandsConfiguration();

                configuration?.Invoke(options);

                //
                // Make all services available to bot commands
                // 
                options.Services = provider;

                var discord = provider.GetRequiredService<IDiscordClientService>().Client;

                var ext = discord.UseSlashCommands(options);
                
                extension?.Invoke(ext);

                //
                // TODO: hook up events
                // 

                return null;
            });

            return services;
        }
    }
}