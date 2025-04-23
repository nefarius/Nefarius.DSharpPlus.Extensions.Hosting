#nullable enable

using System;

using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;

using Microsoft.Extensions.DependencyInjection;

using Nefarius.DSharpPlus.Extensions.Hosting;
using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.Events;
using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.Util;

namespace Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting;

internal sealed class SlashCommandsActivator(
    Action<SlashCommandsConfiguration>? configuration = null,
    Action<SlashCommandsExtension>? extension = null) : IServiceActivator
{
    public void Activate(IServiceProvider provider)
    {
        SlashCommandsConfiguration options = new();

        configuration?.Invoke(options);

        //
        // Make all services available to bot commands
        // 
        options.Services = provider;

        DiscordClient discord = provider.GetRequiredService<IDiscordClientService>().Client;

        SlashCommandsExtension ext = discord.UseSlashCommands(options);

        extension?.Invoke(ext);

        ext.ContextMenuErrored += async delegate(SlashCommandsExtension sender, ContextMenuErrorEventArgs args)
        {
            using IServiceScope scope = provider.CreateScope();

            foreach (IDiscordSlashCommandsEventsSubscriber eventsSubscriber in scope
                         .GetDiscordSlashCommandsEventsSubscriber())
            {
                await eventsSubscriber.SlashCommandsOnContextMenuErrored(sender, args);
            }
        };

        ext.ContextMenuExecuted +=
            async delegate(SlashCommandsExtension sender, ContextMenuExecutedEventArgs args)
            {
                using IServiceScope scope = provider.CreateScope();

                foreach (IDiscordSlashCommandsEventsSubscriber eventsSubscriber in scope
                             .GetDiscordSlashCommandsEventsSubscriber())
                {
                    await eventsSubscriber.SlashCommandsOnContextMenuExecuted(sender, args);
                }
            };

        ext.SlashCommandErrored +=
            async delegate(SlashCommandsExtension sender, SlashCommandErrorEventArgs args)
            {
                using IServiceScope scope = provider.CreateScope();

                foreach (IDiscordSlashCommandsEventsSubscriber eventsSubscriber in scope
                             .GetDiscordSlashCommandsEventsSubscriber())
                {
                    await eventsSubscriber.SlashCommandsOnSlashCommandErrored(sender, args);
                }
            };

        ext.SlashCommandExecuted +=
            async delegate(SlashCommandsExtension sender, SlashCommandExecutedEventArgs args)
            {
                using IServiceScope scope = provider.CreateScope();

                foreach (IDiscordSlashCommandsEventsSubscriber eventsSubscriber in scope
                             .GetDiscordSlashCommandsEventsSubscriber())
                {
                    await eventsSubscriber.SlashCommandsOnSlashCommandExecuted(sender, args);
                }
            };
    }
}