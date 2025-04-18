using System;
using System.Diagnostics.CodeAnalysis;

using DSharpPlus;
using DSharpPlus.VoiceNext;

using Microsoft.Extensions.DependencyInjection;

using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.VoiceNext.Extensions.Hosting;

/// <summary>
///     Extensions methods for <see cref="IServiceCollection" />.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public static class DiscordServiceCollectionExtensions
{
    /// <summary>
    ///     Adds VoiceNext extension to <see cref="IDiscordClientService" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" />.</param>
    /// <param name="configure">The <see cref="VoiceNextConfiguration" />.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddDiscordVoiceNext(
        this IServiceCollection services,
        Action<VoiceNextConfiguration?> configure = null
    )
    {
        services.AddSingleton(typeof(IDiscordExtensionConfiguration), provider =>
        {
            VoiceNextConfiguration options = new();

            configure?.Invoke(options);

            DiscordClient discord = provider.GetRequiredService<IDiscordClientService>().Client;

            discord.UseVoiceNext(options);

            //
            // This is intentional; we don't need this "service", just the execution flow ;)
            // 
            return null;
        });

        return services;
    }
}