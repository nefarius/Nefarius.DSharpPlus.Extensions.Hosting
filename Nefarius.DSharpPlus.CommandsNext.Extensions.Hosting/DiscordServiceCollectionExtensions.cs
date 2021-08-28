using System;
using DSharpPlus.CommandsNext;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Events;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Util;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting
{
    [UsedImplicitly]
    public static class DiscordServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds CommandsNext extension to <see cref="IDiscordClientService"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="CommandsNextConfiguration"/>.</param>
        /// <param name="extension">The <see cref="CommandsNextExtension"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddDiscordCommandsNext(
            this IServiceCollection services,
            Action<CommandsNextConfiguration> configuration,
            Action<CommandsNextExtension?> extension = null
        )
        {
            services.AddSingleton(typeof(IDiscordExtensionConfiguration), provider =>
            {
                var options = new CommandsNextConfiguration();

                configuration(options);

                //
                // Make all services available to bot commands
                // 
                options.Services = provider;

                var discord = provider.GetRequiredService<IDiscordClientService>().Client;

                var ext = discord.UseCommandsNext(options);

                extension?.Invoke(ext);

                ext.CommandExecuted += async delegate (CommandsNextExtension sender, CommandExecutionEventArgs args)
                {
                    using var scope = provider.CreateScope();

                    foreach (var eventsSubscriber in scope.GetDiscordCommandsNextEventsSubscriber())
                        await eventsSubscriber.CommandsOnCommandExecuted(sender, args);
                };

                ext.CommandErrored += async delegate (CommandsNextExtension sender, CommandErrorEventArgs args)
                {
                    using var scope = provider.CreateScope();

                    foreach (var eventsSubscriber in scope.GetDiscordCommandsNextEventsSubscriber())
                        await eventsSubscriber.CommandsOnCommandErrored(sender, args);
                };

                //
                // This is intentional; we don't need this "service", just the execution flow ;)
                // 
                return null;
            });

            return services;
        }

        #region Subscribers

        [UsedImplicitly]
        public static IServiceCollection AddDiscordCommandsNextEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordCommandsNextEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordCommandsNextEventsSubscriber), typeof(T));
        }

        #endregion
    }
}