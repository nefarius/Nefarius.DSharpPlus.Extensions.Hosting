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
internal partial class DiscordService : IDiscordClientService
{
    private readonly IOptions<DiscordConfiguration> _discordOptions;
    private readonly ILoggerFactory _logFactory;

    private readonly ILogger<DiscordService> _logger;

    private readonly IServiceProvider _serviceProvider;

    public DiscordService(
        IServiceProvider serviceProvider,
        ILoggerFactory logFactory,
        ILogger<DiscordService> logger,
        IOptions<DiscordConfiguration> discordOptions
    )
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _discordOptions = discordOptions;
        _logFactory = logFactory;
    }

    /// <summary>
    ///     Gets the <see cref="DiscordClient" />.
    /// </summary>
    public DiscordClient Client { get; private set; }

    internal void Initialize()
    {
        if (_discordOptions.Value is null)
        {
            throw new InvalidOperationException($"{nameof(DiscordConfiguration)} option is required");
        }

        //
        // Grab the content of the user-set intents and merge them with what the subscribers need
        // 
        PropertyInfo property = typeof(DiscordConfiguration).GetProperty("Intents");

        if (property is null)
        {
            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("Property 'Intents' not found in class 'DiscordConfiguration'.");
        }

        if (property.DeclaringType is null)
        {
            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("Declaring type of 'Intents' property not found.");
        }

        property = property.DeclaringType.GetProperty(nameof(DiscordConfiguration.Intents));
        DiscordIntents? intents = (DiscordIntents?)property!.GetValue(_discordOptions.Value,
            BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);

        using IServiceScope serviceScope = _serviceProvider.CreateScope();

        intents = BuildIntents(serviceScope, intents!.Value);

        DiscordConfiguration configuration = new(_discordOptions.Value)
        {
            //
            // Overwrite with DI configured logging factory
            // 
            LoggerFactory = _logFactory,
            //
            // Use merged intents
            // 
            Intents = intents.Value
        };

        Client = new DiscordClient(configuration);

        //
        // Load options that should load in before Connect call
        // TODO: this is a messy workaround, come up with something smarter!
        // 
        _serviceProvider.GetServices<IDiscordExtensionConfiguration>();

        HookEvents();
    }
}