﻿#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

using DSharpPlus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Nefarius.DSharpPlus.Extensions.Hosting;

/// <summary>
///     Extensions methods for <see cref="IServiceCollection" />.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
public static partial class DiscordServiceCollectionExtensions
{
    /// <summary>
    ///     Registers a <see cref="IDiscordClientService" /> with <see cref="DiscordConfiguration" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" />.</param>
    /// <param name="configure">The <see cref="DiscordConfiguration" />.</param>
    /// <param name="autoRegisterSubscribers">
    ///     If true, classes with subscriber attributes will get registered as event
    ///     subscribers automatically. This is the default.
    /// </param>
    /// <param name="connectOptions">Optional <see cref="DiscordClientConnectOptions" />.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddDiscord(
        this IServiceCollection services,
        Action<DiscordConfiguration> configure,
        bool autoRegisterSubscribers = true,
        Action<DiscordClientConnectOptions>? connectOptions = null
    )
    {
        services.Configure(configure);

        DiscordClientConnectOptions options = new();
        connectOptions?.Invoke(options);
        services.TryAddSingleton<IOptions<DiscordClientConnectOptions>>(
            new OptionsWrapper<DiscordClientConnectOptions>(options));

        services.TryAddSingleton<IDiscordClientService, DiscordService>();

        if (!autoRegisterSubscribers)
        {
            return services;
        }

        RegisterSubscribers(services);

        return services;
    }

    /// <summary>
    ///     Registers a <see cref="DiscordHostedService" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" />.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddDiscordHostedService(
        this IServiceCollection services
    )
    {
        return services.AddHostedService<DiscordHostedService>();
    }
}