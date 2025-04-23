#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

using DSharpPlus.CommandsNext;

using Microsoft.Extensions.DependencyInjection;

using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Attributes;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Events;
using Nefarius.DSharpPlus.Extensions.Hosting;
using Nefarius.DSharpPlus.Extensions.Hosting.Util;

namespace Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting;

/// <summary>
///     Extensions methods for <see cref="IServiceCollection" />.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
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
    public static IServiceCollection AddDiscordCommandsNext(
        this IServiceCollection services,
        Action<CommandsNextConfiguration> configuration,
        Action<CommandsNextExtension>? extension = null,
        bool autoRegisterSubscribers = true
    )
    {
        services.AddSingleton<IServiceActivator, CommandsNextActivator>(_ =>
            new CommandsNextActivator(configuration, extension));

        if (!autoRegisterSubscribers)
        {
            return services;
        }

        foreach (Type type in AssemblyTypeHelper.GetTypesWith<DiscordCommandsNextEventsSubscriberAttribute>())
        {
            services.AddDiscordCommandsNextEventsSubscriber(type);
        }

        return services;
    }

    #region Subscribers

    /// <summary>
    ///     Registers a CommandsNext events subscriber.
    /// </summary>
    /// <typeparam name="T">Implementation of <see cref="IDiscordCommandsNextEventsSubscriber" />.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddDiscordCommandsNextEventsSubscriber<T>(this IServiceCollection services)
        where T : IDiscordCommandsNextEventsSubscriber
    {
        return services.AddDiscordCommandsNextEventsSubscriber(typeof(T));
    }

    /// <summary>
    ///     Registers a CommandsNext events subscriber.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="t">Implementation of <see cref="IDiscordCommandsNextEventsSubscriber" />.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddDiscordCommandsNextEventsSubscriber(this IServiceCollection services,
        Type t)
    {
        return services.AddScoped(typeof(IDiscordCommandsNextEventsSubscriber), t);
    }

    #endregion
}