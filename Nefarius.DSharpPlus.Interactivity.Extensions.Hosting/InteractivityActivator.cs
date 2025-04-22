#nullable enable

using System;

using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

using Microsoft.Extensions.DependencyInjection;

using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.Interactivity.Extensions.Hosting;

internal sealed class InteractivityActivator(
    Action<InteractivityConfiguration> configuration,
    Action<InteractivityExtension>? extension = null)
    : IServiceActivator
{
    public void Activate(IServiceProvider provider)
    {
        InteractivityConfiguration options = new();

        configuration(options);

        DiscordClient discord = provider.GetRequiredService<IDiscordClientService>().Client;

        InteractivityExtension ext = discord.UseInteractivity(options);

        extension?.Invoke(ext);
    }
}