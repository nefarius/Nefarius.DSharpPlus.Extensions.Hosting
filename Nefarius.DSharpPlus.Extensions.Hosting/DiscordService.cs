using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DSharpPlus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    /// <summary>
    ///     Wraps a <see cref="DiscordClient"/> as a service.
    /// </summary>
    public interface IDiscordClientService
    {
        /// <summary>
        ///     The underlying <see cref="DiscordClient"/>.
        /// </summary>
        public DiscordClient Client { get; }
    }

    /// <summary>
    ///     An implementation of <see cref="IDiscordClientService"/>.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class DiscordService : IDiscordClientService
    {
	    private readonly ILoggerFactory _logFactory;

	    private readonly ILogger<DiscordService> _logger;

	    private readonly IOptions<DiscordConfiguration> _discordOptions;

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
        ///     Gets the <see cref="DiscordClient"/>.
        /// </summary>
        public DiscordClient Client { get; private set; }

        internal void Initialize()
        {
            if (_discordOptions.Value is null)
                throw new InvalidOperationException($"{nameof(DiscordConfiguration)} option is required");

            //
            // Grab the content of the user-set intents and merge them with what the subscribers need
            // 
            var property = typeof(DiscordConfiguration).GetProperty("Intents");
            property = property.DeclaringType.GetProperty("Intents");
            var intents = (DiscordIntents)property.GetValue(_discordOptions.Value,
                BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            
            using var serviceScope = _serviceProvider.CreateScope();

            intents = BuildIntents(serviceScope, intents);

            var configuration = new DiscordConfiguration(_discordOptions.Value)
            {
                //
                // Overwrite with DI configured logging factory
                // 
                LoggerFactory = _logFactory,
                //
                // Use merged intents
                // 
                Intents = intents
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
}