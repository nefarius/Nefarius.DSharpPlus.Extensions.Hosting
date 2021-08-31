using System;
using DSharpPlus.CommandsNext;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Attributes;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Events;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Util;
using Nefarius.DSharpPlus.Extensions.Hosting;
using Nefarius.DSharpPlus.Extensions.Hosting.Util;
using OpenTracing;

namespace Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting
{
    [UsedImplicitly]
    public static class DiscordServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds CommandsNext extension to <see cref="IDiscordClientService" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="CommandsNextConfiguration" />.</param>
        /// <param name="extension">The <see cref="CommandsNextExtension" />.</param>
        /// <param name="autoRegisterSubscribers">
        ///     If true, classes with subscriber attributes will get registered as event
        ///     subscribers automatically. This is the default.
        /// </param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddDiscordCommandsNext(
            this IServiceCollection services,
            Action<CommandsNextConfiguration> configuration,
            Action<CommandsNextExtension?> extension = null,
            bool autoRegisterSubscribers = true
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

                ext.CommandExecuted += async delegate(CommandsNextExtension sender, CommandExecutionEventArgs args)
                {
                    using var workScope = provider.GetRequiredService<ITracer>()
                        .BuildSpan(nameof(ext.CommandExecuted))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Command.Name", args.Command.Name);

                    using var scope = provider.CreateScope();

                    foreach (var eventsSubscriber in scope.GetDiscordCommandsNextEventsSubscriber())
                        await eventsSubscriber.CommandsOnCommandExecuted(sender, args);
                };

                ext.CommandErrored += async delegate(CommandsNextExtension sender, CommandErrorEventArgs args)
                {
                    using var workScope = provider.GetRequiredService<ITracer>()
                        .BuildSpan(nameof(ext.CommandErrored))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Command.Name", args.Command.Name);

                    using var scope = provider.CreateScope();

                    foreach (var eventsSubscriber in scope.GetDiscordCommandsNextEventsSubscriber())
                        await eventsSubscriber.CommandsOnCommandErrored(sender, args);
                };

                //
                // This is intentional; we don't need this "service", just the execution flow ;)
                // 
                return null;
            });

            if (!autoRegisterSubscribers)
                return services;

            foreach (var type in AssemblyTypeHelper.GetTypesWith<DiscordCommandsNextEventsSubscriberAttribute>())
                services.AddDiscordCommandsNextEventsSubscriber(type);

            return services;
        }

        #region Subscribers

        [UsedImplicitly]
        public static IServiceCollection AddDiscordCommandsNextEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordCommandsNextEventsSubscriber
        {
            return services.AddDiscordCommandsNextEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordCommandsNextEventsSubscriber(this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordCommandsNextEventsSubscriber), t);
        }

        #endregion
    }
}