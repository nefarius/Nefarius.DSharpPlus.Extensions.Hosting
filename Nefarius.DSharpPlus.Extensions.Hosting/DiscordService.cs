using System;
using System.Reflection;
using DSharpPlus;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTracing;

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
    [UsedImplicitly]
    public partial class DiscordService : IDiscordClientService
    {
        protected readonly ILoggerFactory LogFactory;

        protected readonly ILogger<DiscordService> Logger;

        protected readonly IOptions<DiscordConfiguration> DiscordOptions;
        
        protected readonly IServiceProvider ServiceProvider;

        protected readonly ITracer Tracer;

        public DiscordService(
            IServiceProvider serviceProvider,
            ILoggerFactory logFactory,
            ILogger<DiscordService> logger,
            ITracer tracer,
            IOptions<DiscordConfiguration> discordOptions
        )
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
            Tracer = tracer;
            DiscordOptions = discordOptions;
            LogFactory = logFactory;
        }

        public DiscordClient Client { get; private set; }

        internal void Initialize()
        {
            if (DiscordOptions.Value is null)
                throw new InvalidOperationException($"{nameof(DiscordConfiguration)} option is required");

            //
            // Grab the content of the user-set intents and merge them with what the subscribers need
            // 
            var property = typeof(DiscordConfiguration).GetProperty("Intents");
            property = property.DeclaringType.GetProperty("Intents");
            var intents = (DiscordIntents)property.GetValue(DiscordOptions.Value,
                BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            
            using var serviceScope = ServiceProvider.CreateScope();

            intents = BuildIntents(serviceScope, intents);

            var configuration = new DiscordConfiguration(DiscordOptions.Value)
            {
                //
                // Overwrite with DI configured logging factory
                // 
                LoggerFactory = LogFactory,
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
            ServiceProvider.GetServices<IDiscordExtensionConfiguration>();

            HookEvents();
        }
    }
}