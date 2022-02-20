using System;
using System.Linq;
using System.Reflection;
using DSharpPlus;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nefarius.DSharpPlus.Extensions.Hosting.Util;
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

            using var serviceScope = ServiceProvider.CreateScope();

            #region Subscriber services

            var channelEventsSubscriber = serviceScope.GetDiscordChannelEventsSubscribers();

            var guildEventSubscribers = serviceScope.GetDiscordGuildEventsSubscribers();

            var guildBanEventsSubscriber = serviceScope.GetDiscordGuildBanEventsSubscribers();

            var guildMemberEventsSubscriber = serviceScope.GetDiscordGuildMemberEventsSubscribers();

            var guildRoleEventsSubscriber = serviceScope.GetDiscordGuildRoleEventsSubscribers();

            var inviteEventsSubscriber = serviceScope.GetDiscordInviteEventsSubscribers();

            var messageEventsSubscriber = serviceScope.GetDiscordMessageEventsSubscribers();

            var messageReactionAddedEventsSubscriber = serviceScope.GetDiscordMessageReactionAddedEventsSubscribers();

            var presenceUserEventsSubscriber = serviceScope.GetDiscordPresenceUserEventsSubscribers();

            var voiceEventsSubscriber = serviceScope.GetDiscordVoiceEventsSubscribers();

            #endregion

            #region Build intents

            //
            // Grab the content of the user-set intents and merge them with what the subscribers need
            // 
            var property = typeof(DiscordConfiguration).GetProperty("Intents");
            property = property.DeclaringType.GetProperty("Intents");
            var intents = (DiscordIntents)property.GetValue(DiscordOptions.Value,
                BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);

            //
            // Merge/enrich intents the user requested with those the subscribers require
            // 

            if (channelEventsSubscriber.Any())
                intents |= DiscordIntents.Guilds;
            if (guildEventSubscribers.Any())
                intents |= DiscordIntents.Guilds | DiscordIntents.GuildEmojis;
            if (guildBanEventsSubscriber.Any())
                intents |= DiscordIntents.GuildBans;
            if (guildMemberEventsSubscriber.Any())
                intents |= DiscordIntents.GuildMembers;
            if (guildRoleEventsSubscriber.Any())
                intents |= DiscordIntents.Guilds;
            if (inviteEventsSubscriber.Any())
                intents |= DiscordIntents.GuildInvites;
            if (messageEventsSubscriber.Any())
                intents |= DiscordIntents.GuildMessages;
            if (messageReactionAddedEventsSubscriber.Any())
                intents |= DiscordIntents.GuildMessageReactions;
            if (presenceUserEventsSubscriber.Any())
                intents |= DiscordIntents.GuildPresences;
            if (voiceEventsSubscriber.Any())
                intents |= DiscordIntents.GuildVoiceStates;

            #endregion

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