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
        /// <param name="configure">The <see cref="DiscordSlashCommandsOptions" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddDiscordSlashCommands(
            this IServiceCollection services,
            Action<DiscordSlashCommandsOptions?> configure
        )
        {
            services.AddSingleton(typeof(IDiscordExtensionConfiguration), provider =>
            {
                var options = new DiscordSlashCommandsOptions();

                if (configure != null)
                    configure(options);
                else
                    options.Configuration = new SlashCommandsConfiguration();

                //
                // Make all services available to bot commands
                // 
                options.Configuration.Services = provider;

                var discord = provider.GetRequiredService<IDiscordClientService>().Client;

                var ext = discord.UseSlashCommands(options.Configuration);

                foreach (var (type, guildId) in options.CommandModules) ext.RegisterCommands(type, guildId);

                //
                // TODO: hook up events
                // 

                return options;
            });

            return services;
        }
    }
}