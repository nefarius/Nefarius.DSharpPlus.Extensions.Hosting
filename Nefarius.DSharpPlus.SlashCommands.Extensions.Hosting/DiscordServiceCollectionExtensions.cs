#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

using DSharpPlus.SlashCommands;

using Microsoft.Extensions.DependencyInjection;

using Nefarius.DSharpPlus.Extensions.Hosting;
using Nefarius.DSharpPlus.Extensions.Hosting.Util;
using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.Attributes;
using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.Events;

namespace Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting;

/// <summary>
///     Extensions methods for <see cref="IServiceCollection" />.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
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
    public static IServiceCollection AddDiscordSlashCommands(
        this IServiceCollection services,
        Action<SlashCommandsConfiguration>? configuration = null,
        Action<SlashCommandsExtension>? extension = null,
        bool autoRegisterSubscribers = true
    )
    {
        services.AddSingleton<IServiceActivator, SlashCommandsActivator>(_ =>
            new SlashCommandsActivator(configuration, extension));

        if (!autoRegisterSubscribers)
        {
            return services;
        }

        foreach (Type type in AssemblyTypeHelper.GetTypesWith<DiscordSlashCommandsEventsSubscriberAttribute>())
        {
            services.AddDiscordSlashCommandsEventsSubscriber(type);
        }

        return services;
    }

    #region Subscribers

    /// <summary>
    ///     Registers an event subscriber implementation.
    /// </summary>
    /// <typeparam name="T">An implementation of <see cref="IDiscordSlashCommandsEventsSubscriber" />.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" />.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddDiscordSlashCommandsEventsSubscriber<T>(this IServiceCollection services)
        where T : IDiscordSlashCommandsEventsSubscriber
    {
        return services.AddDiscordSlashCommandsEventsSubscriber(typeof(T));
    }

    /// <summary>
    ///     Registers an event subscriber implementation.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" />.</param>
    /// <param name="t">The type of the subscriber implementation.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddDiscordSlashCommandsEventsSubscriber(this IServiceCollection services,
        Type t)
    {
        return services.AddScoped(typeof(IDiscordSlashCommandsEventsSubscriber), t);
    }

    #endregion
}