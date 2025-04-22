#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using DSharpPlus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Nefarius.DSharpPlus.Extensions.Hosting;

/// <summary>
///     Wraps a <see cref="DiscordClient" /> as a service.
/// </summary>
public interface IDiscordClientService
{
    /// <summary>
    ///     The underlying <see cref="DiscordClient" />.
    /// </summary>
    public DiscordClient Client { get; }
}

/// <summary>
///     An implementation of <see cref="IDiscordClientService" />.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public partial class DiscordService(
    IServiceProvider serviceProvider,
    ILoggerFactory logFactory,
    IOptions<DiscordConfiguration> discordOptions)
    : IDiscordClientService
{
    /// <summary>
    ///     Gets the <see cref="DiscordClient" />.
    /// </summary>
    public DiscordClient Client { get; private set; } = null!;

    /// <summary>
    ///     Discover intents, initialize <see cref="DiscordClient"/> and hook events to subscribers.
    /// </summary>
    internal void Initialize()
    {
        if (discordOptions.Value is null)
        {
            throw new InvalidOperationException($"{nameof(DiscordConfiguration)} option is required");
        }

        //
        // Grab the content of the user-set intents and merge them with what the subscribers need
        // 
        PropertyInfo? property = typeof(DiscordConfiguration).GetProperty(nameof(DiscordConfiguration.Intents));
        property = property!.DeclaringType!.GetProperty(nameof(DiscordConfiguration.Intents));
        DiscordIntents intents = (DiscordIntents)property!.GetValue(
            discordOptions.Value,
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, null, null
        );

        using IServiceScope serviceScope = serviceProvider.CreateScope();

        intents = BuildIntents(serviceScope, intents);

        DiscordConfiguration configuration = new(discordOptions.Value)
        {
            //
            // Overwrite with DI configured logging factory
            // 
            LoggerFactory = logFactory,
            //
            // Use merged intents
            // 
            Intents = intents
        };

        Client = new DiscordClient(configuration);

        //
        // Load options that should load in before Connect call
        // 
        foreach (IServiceActivator activator in serviceProvider.GetServices<IServiceActivator>())
        {
            activator.Activate(serviceProvider);
        }

        HookEvents();
    }
}