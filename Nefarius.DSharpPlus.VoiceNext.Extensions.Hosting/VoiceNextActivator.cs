#nullable enable

using System;

using DSharpPlus;
using DSharpPlus.VoiceNext;

using Microsoft.Extensions.DependencyInjection;

using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.VoiceNext.Extensions.Hosting;

internal sealed class VoiceNextActivator(
    Action<VoiceNextConfiguration>? configure = null,
    Action<VoiceNextExtension>? extension = null) : IServiceActivator
{
    public void Activate(IServiceProvider provider)
    {
        VoiceNextConfiguration options = new();

        configure?.Invoke(options);

        DiscordClient discord = provider.GetRequiredService<IDiscordClientService>().Client;

        VoiceNextExtension? ext = discord.UseVoiceNext(options);

        extension?.Invoke(ext);
    }
}