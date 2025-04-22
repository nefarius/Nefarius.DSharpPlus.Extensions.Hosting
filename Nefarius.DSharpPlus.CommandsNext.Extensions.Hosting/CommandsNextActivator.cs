#nullable enable

using System;

using DSharpPlus;
using DSharpPlus.CommandsNext;

using Microsoft.Extensions.DependencyInjection;

using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Events;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Util;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting;

internal sealed class CommandsNextActivator(
    Action<CommandsNextConfiguration> configuration,
    Action<CommandsNextExtension>? extension = null) : IServiceActivator
{
    public void Activate(IServiceProvider provider)
    {
        CommandsNextConfiguration options = new();

        configuration(options);

        //
        // Make all services available to bot commands
        // 
        options.Services = provider;

        DiscordClient discord = provider.GetRequiredService<IDiscordClientService>().Client;

        CommandsNextExtension ext = discord.UseCommandsNext(options);

        extension?.Invoke(ext);

        ext.CommandExecuted += async delegate(CommandsNextExtension sender, CommandExecutionEventArgs args)
        {
            using IServiceScope scope = provider.CreateScope();

            foreach (IDiscordCommandsNextEventsSubscriber eventsSubscriber in scope
                         .GetDiscordCommandsNextEventsSubscriber())
            {
                await eventsSubscriber.CommandsOnCommandExecuted(sender, args);
            }
        };

        ext.CommandErrored += async delegate(CommandsNextExtension sender, CommandErrorEventArgs args)
        {
            using IServiceScope scope = provider.CreateScope();

            foreach (IDiscordCommandsNextEventsSubscriber eventsSubscriber in scope
                         .GetDiscordCommandsNextEventsSubscriber())
            {
                await eventsSubscriber.CommandsOnCommandErrored(sender, args);
            }
        };
    }
}