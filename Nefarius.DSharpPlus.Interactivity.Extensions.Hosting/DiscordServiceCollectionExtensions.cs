#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

using DSharpPlus.Interactivity;

using Microsoft.Extensions.DependencyInjection;

using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.Interactivity.Extensions.Hosting;

/// <summary>
///     Extensions methods for <see cref="IServiceCollection" />.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
public static class DiscordServiceCollectionExtensions
{
    /// <summary>
    ///     Adds Interactivity extension to <see cref="IDiscordClientService" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" />.</param>
    /// <param name="configuration">The <see cref="InteractivityConfiguration" />.</param>
    /// <param name="extension">The <see cref="InteractivityExtension" />.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddDiscordInteractivity(
        this IServiceCollection services,
        Action<InteractivityConfiguration> configuration,
        Action<InteractivityExtension>? extension = null
    )
    {
        services.AddSingleton<IServiceActivator, InteractivityActivator>(_ =>
            new InteractivityActivator(configuration, extension));

        return services;
    }
}