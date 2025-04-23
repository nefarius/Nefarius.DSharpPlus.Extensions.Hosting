#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

using DSharpPlus.VoiceNext;

using Microsoft.Extensions.DependencyInjection;

using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.VoiceNext.Extensions.Hosting;

/// <summary>
///     Extensions methods for <see cref="IServiceCollection" />.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public static class DiscordServiceCollectionExtensions
{
    /// <summary>
    ///     Adds VoiceNext extension to <see cref="IDiscordClientService" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" />.</param>
    /// <param name="configure">The <see cref="VoiceNextConfiguration" />.</param>
    /// <param name="extension">The <see cref="VoiceNextExtension" />.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddDiscordVoiceNext(
        this IServiceCollection services,
        Action<VoiceNextConfiguration>? configure = null,
        Action<VoiceNextExtension>? extension = null
    )
    {
        services.AddSingleton<IServiceActivator, VoiceNextActivator>(_ => new VoiceNextActivator(configure, extension));

        return services;
    }
}