using System;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.Extensions.Hosting;
using Nefarius.DSharpPlus.Extensions.Hosting.Util;
using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.Attributes;
using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.Events;
using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.Util;
using OpenTracing;

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
        /// <param name="extension">The <see cref="SlashCommandsExtension" />.</param>
        /// <param name="autoRegisterSubscribers">
        ///     If true, classes with subscriber attributes will get registered as event
        ///     subscribers automatically. This is the default.
        /// </param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        [UsedImplicitly]
        public static IServiceCollection AddDiscordSlashCommands(
            this IServiceCollection services,
            Action<SlashCommandsConfiguration?> configuration = null,
            Action<SlashCommandsExtension?> extension = null,
            bool autoRegisterSubscribers = true
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

                ext.ContextMenuErrored += async delegate(SlashCommandsExtension sender, ContextMenuErrorEventArgs args)
                {
                    using var workScope = provider.GetRequiredService<ITracer>()
                        .BuildSpan(nameof(ext.ContextMenuErrored))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Context.CommandName", args.Context.CommandName);

                    using var scope = provider.CreateScope();

                    foreach (var eventsSubscriber in scope.GetDiscordSlashCommandsEventsSubscriber())
                        await eventsSubscriber.SlashCommandsOnContextMenuErrored(sender, args);
                };

                ext.ContextMenuExecuted +=
                    async delegate(SlashCommandsExtension sender, ContextMenuExecutedEventArgs args)
                    {
                        using var workScope = provider.GetRequiredService<ITracer>()
                            .BuildSpan(nameof(ext.ContextMenuExecuted))
                            .IgnoreActiveSpan()
                            .StartActive(true);
                        workScope.Span.SetTag("Context.CommandName", args.Context.CommandName);

                        using var scope = provider.CreateScope();

                        foreach (var eventsSubscriber in scope.GetDiscordSlashCommandsEventsSubscriber())
                            await eventsSubscriber.SlashCommandsOnContextMenuExecuted(sender, args);
                    };

                ext.SlashCommandErrored +=
                    async delegate(SlashCommandsExtension sender, SlashCommandErrorEventArgs args)
                    {
                        using var workScope = provider.GetRequiredService<ITracer>()
                            .BuildSpan(nameof(ext.SlashCommandErrored))
                            .IgnoreActiveSpan()
                            .StartActive(true);
                        workScope.Span.SetTag("Context.CommandName", args.Context.CommandName);

                        using var scope = provider.CreateScope();

                        foreach (var eventsSubscriber in scope.GetDiscordSlashCommandsEventsSubscriber())
                            await eventsSubscriber.SlashCommandsOnSlashCommandErrored(sender, args);
                    };

                ext.SlashCommandExecuted +=
                    async delegate(SlashCommandsExtension sender, SlashCommandExecutedEventArgs args)
                    {
                        using var workScope = provider.GetRequiredService<ITracer>()
                            .BuildSpan(nameof(ext.SlashCommandExecuted))
                            .IgnoreActiveSpan()
                            .StartActive(true);
                        workScope.Span.SetTag("Context.CommandName", args.Context.CommandName);

                        using var scope = provider.CreateScope();

                        foreach (var eventsSubscriber in scope.GetDiscordSlashCommandsEventsSubscriber())
                            await eventsSubscriber.SlashCommandsOnSlashCommandExecuted(sender, args);
                    };

                //
                // This is intentional; we don't need this "service", just the execution flow ;)
                // 
                return null;
            });

            if (!autoRegisterSubscribers)
                return services;

            foreach (var type in AssemblyTypeHelper.GetTypesWith<DiscordSlashCommandsEventsSubscriberAttribute>())
                services.AddDiscordSlashCommandsEventsSubscriber(type);

            return services;
        }

        #region Subscribers

        [UsedImplicitly]
        public static IServiceCollection AddDiscordSlashCommandsEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordSlashCommandsEventsSubscriber
        {
            return services.AddDiscordSlashCommandsEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordSlashCommandsEventsSubscriber(this IServiceCollection services,
            Type t)
        {
            return services.AddScoped(typeof(IDiscordSlashCommandsEventsSubscriber), t);
        }

        #endregion
    }
}