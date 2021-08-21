using System;
using System.Linq;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;
using OpenTracing;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    public interface IDiscordClientService
    {
        public DiscordClient Client { get; }

        public void Initialize();
    }

    [UsedImplicitly]
    public class DiscordService : IDiscordClientService
    {
        protected readonly ILoggerFactory LogFactory;

        protected readonly ILogger<DiscordService> Logger;

        protected readonly IOptions<DiscordServiceOptions> Options;

        protected readonly IServiceProvider ServiceProvider;

        protected readonly ITracer Tracer;

        public DiscordService(
            IServiceProvider serviceProvider,
            ILoggerFactory logFactory,
            ILogger<DiscordService> logger,
            ITracer tracer,
            IOptions<DiscordServiceOptions> options
        )
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
            Tracer = tracer;
            Options = options;
            LogFactory = logFactory;
        }

        public DiscordClient Client { get; private set; }

        public void Initialize()
        {
            using var scope = ServiceProvider.CreateScope();

            #region Subscriber services
            
            var channelEventsSubscriber = scope.ServiceProvider
                .GetServices(typeof(IDiscordChannelEventsSubscriber))
                .Cast<IDiscordChannelEventsSubscriber>()
                .ToList();

            var guildEventSubscribers = scope.ServiceProvider
                .GetServices(typeof(IDiscordGuildEventsSubscriber))
                .Cast<IDiscordGuildEventsSubscriber>()
                .ToList();

            var guildBanEventsSubscriber = scope.ServiceProvider
                .GetServices(typeof(IDiscordGuildBanEventsSubscriber))
                .Cast<IDiscordGuildBanEventsSubscriber>()
                .ToList();

            var guildMemberEventsSubscriber = scope.ServiceProvider
                .GetServices(typeof(IDiscordGuildMemberEventsSubscriber))
                .Cast<IDiscordGuildMemberEventsSubscriber>()
                .ToList();

            var guildRoleEventsSubscriber = scope.ServiceProvider
                .GetServices(typeof(IDiscordGuildRoleEventsSubscriber))
                .Cast<IDiscordGuildRoleEventsSubscriber>()
                .ToList();

            var inviteEventsSubscriber = scope.ServiceProvider
                .GetServices(typeof(IDiscordInviteEventsSubscriber))
                .Cast<IDiscordInviteEventsSubscriber>()
                .ToList();

            var messageEventsSubscriber = scope.ServiceProvider
                .GetServices(typeof(IDiscordMessageEventsSubscriber))
                .Cast<IDiscordMessageEventsSubscriber>()
                .ToList();

            var messageReactionAddedEventsSubscriber = scope.ServiceProvider
                .GetServices(typeof(IDiscordMessageReactionAddedEventsSubscriber))
                .Cast<IDiscordMessageReactionAddedEventsSubscriber>()
                .ToList();

            var presenceUserEventsSubscriber = scope.ServiceProvider
                .GetServices(typeof(IDiscordPresenceUserEventsSubscriber))
                .Cast<IDiscordPresenceUserEventsSubscriber>()
                .ToList();

            var voiceEventsSubscriber = scope.ServiceProvider
                .GetServices(typeof(IDiscordVoiceEventsSubscriber))
                .Cast<IDiscordVoiceEventsSubscriber>()
                .ToList();

            var miscEventsSubscriber = scope.ServiceProvider
                .GetServices(typeof(IDiscordMiscEventsSubscriber))
                .Cast<IDiscordMiscEventsSubscriber>()
                .ToList();

            #endregion

            #region Build intents

            var intents = DiscordIntents.AllUnprivileged;

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

            var configuration = new DiscordConfiguration(Options.Value.Configuration)
            {
                //
                // Overwrite with DI configured logging factory
                // 
                LoggerFactory = LogFactory,
                Intents = intents
            };

            Client = new DiscordClient(configuration);

            if (Options.Value.Interactivity != null)
                Client.UseInteractivity(Options.Value.Interactivity);

            if (Options.Value.CommandsNext != null)
            {
                var commandsNext = new CommandsNextConfiguration(Options.Value.CommandsNext)
                {
                    //
                    // Overwrite with DI provider to have all other services available to command modules
                    // 
                    Services = ServiceProvider
                };

                var ext = Client.UseCommandsNext(commandsNext);

                foreach (var module in Options.Value.CommandModules)
                {
                    ext.RegisterCommands(module);
                }
            }

            #region Channel

            foreach (var subscriber in channelEventsSubscriber)
            {
                Client.ChannelCreated += async delegate (DiscordClient sender, ChannelCreateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.ChannelCreated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);

                    await subscriber.DiscordOnChannelCreated(sender, args);
                };

                Client.ChannelUpdated += async delegate (DiscordClient sender, ChannelUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.ChannelUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("ChannelBefore.Id", args.ChannelBefore.Id);
                    workScope.Span.SetTag("ChannelAfter.Id", args.ChannelAfter.Id);

                    await subscriber.DiscordOnChannelUpdated(sender, args);
                };

                Client.ChannelDeleted += async delegate (DiscordClient sender, ChannelDeleteEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.ChannelDeleted))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);

                    await subscriber.DiscordOnChannelDeleted(sender, args);
                };

                Client.DmChannelDeleted += async delegate (DiscordClient sender, DmChannelDeleteEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.DmChannelDeleted))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);

                    await subscriber.DiscordOnDmChannelDeleted(sender, args);
                };

                Client.ChannelPinsUpdated += async delegate (DiscordClient sender, ChannelPinsUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.ChannelPinsUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);

                    await subscriber.DiscordOnChannelPinsUpdated(sender, args);
                };
            }

            #endregion

            #region Guild

            foreach (var subscriber in guildEventSubscribers)
            {
                Client.GuildCreated += async delegate (DiscordClient sender, GuildCreateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildCreated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);

                    await subscriber.DiscordOnGuildCreated(sender, args);
                };

                Client.GuildAvailable += async delegate (DiscordClient sender, GuildCreateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildAvailable))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);

                    await subscriber.DiscordOnGuildAvailable(sender, args);
                };

                Client.GuildUpdated += async delegate (DiscordClient sender, GuildUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.GuildBefore.Id);

                    await subscriber.DiscordOnGuildUpdated(sender, args);
                };

                Client.GuildDeleted += async delegate (DiscordClient sender, GuildDeleteEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildDeleted))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);

                    await subscriber.DiscordOnGuildDeleted(sender, args);
                };

                Client.GuildUnavailable += async delegate (DiscordClient sender, GuildDeleteEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildUnavailable))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);

                    await subscriber.DiscordOnGuildUnavailable(sender, args);
                };

                Client.GuildDownloadCompleted +=
                    async delegate (DiscordClient sender, GuildDownloadCompletedEventArgs args)
                    {
                        using var workScope = Tracer
                            .BuildSpan(nameof(Client.GuildDownloadCompleted))
                            .IgnoreActiveSpan()
                            .StartActive(true);

                        await subscriber.DiscordOnGuildDownloadCompleted(sender, args);
                    };

                Client.GuildEmojisUpdated += async delegate (DiscordClient sender, GuildEmojisUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildEmojisUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);

                    await subscriber.DiscordOnGuildEmojisUpdated(sender, args);
                };

                Client.GuildStickersUpdated += async delegate (DiscordClient sender, GuildStickersUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildStickersUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);

                    await subscriber.DiscordOnGuildStickersUpdated(sender, args);
                };

                Client.GuildIntegrationsUpdated +=
                    async delegate (DiscordClient sender, GuildIntegrationsUpdateEventArgs args)
                    {
                        using var workScope = Tracer
                            .BuildSpan(nameof(Client.GuildIntegrationsUpdated))
                            .IgnoreActiveSpan()
                            .StartActive(true);
                        workScope.Span.SetTag("Guild.Id", args.Guild.Id);

                        await subscriber.DiscordOnGuildIntegrationsUpdated(sender, args);
                    };
            }

            #endregion

            #region Guild Ban

            foreach (var subscriber in guildBanEventsSubscriber)
            {
                Client.GuildBanAdded += async delegate (DiscordClient sender, GuildBanAddEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildBanAdded))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Member.Id", args.Member.Id);

                    await subscriber.DiscordOnGuildBanAdded(sender, args);
                };

                Client.GuildBanRemoved += async delegate (DiscordClient sender, GuildBanRemoveEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildBanRemoved))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Member.Id", args.Member.Id);

                    await subscriber.DiscordOnGuildBanRemoved(sender, args);
                };
            }

            #endregion

            #region Guild Member

            foreach (var subscriber in guildMemberEventsSubscriber)
            {
                Client.GuildMemberAdded += async delegate (DiscordClient sender, GuildMemberAddEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildMemberAdded))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Member.Id", args.Member.Id);

                    await subscriber.DiscordOnGuildMemberAdded(sender, args);
                };

                Client.GuildMemberRemoved += async delegate (DiscordClient sender, GuildMemberRemoveEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildMemberRemoved))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Member.Id", args.Member.Id);

                    await subscriber.DiscordOnGuildMemberRemoved(sender, args);
                };

                Client.GuildMemberUpdated += async delegate (DiscordClient sender, GuildMemberUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildMemberUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Member.Id", args.Member.Id);

                    await subscriber.DiscordOnGuildMemberUpdated(sender, args);
                };
            }

            #endregion

            #region Guild Role

            foreach (var subscriber in guildRoleEventsSubscriber)
            {
                Client.GuildRoleCreated += async delegate (DiscordClient sender, GuildRoleCreateEventArgs args)
                {
                    using var workScope = Tracer
                    .BuildSpan(nameof(Client.GuildRoleCreated))
                    .IgnoreActiveSpan()
                    .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Role.Id", args.Role.Id);

                    await subscriber.DiscordOnGuildRoleCreated(sender, args);
                };

                Client.GuildRoleUpdated += async delegate (DiscordClient sender, GuildRoleUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildRoleUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("RoleBefore.Id", args.RoleBefore.Id);
                    workScope.Span.SetTag("RoleAfter.Id", args.RoleAfter.Id);

                    await subscriber.DiscordOnGuildRoleUpdated(sender, args);
                };

                Client.GuildRoleDeleted += async delegate (DiscordClient sender, GuildRoleDeleteEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildRoleDeleted))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Role.Id", args.Role.Id);

                    await subscriber.DiscordOnGuildRoleDeleted(sender, args);
                };
            }

            #endregion

            #region Invite

            foreach (var subscriber in inviteEventsSubscriber)
            {
                Client.InviteCreated += async delegate (DiscordClient sender, InviteCreateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.InviteCreated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("Invite.Code", args.Invite.Code);

                    await subscriber.DiscordOnInviteCreated(sender, args);
                };

                Client.InviteDeleted += async delegate (DiscordClient sender, InviteDeleteEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.InviteDeleted))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("Invite.Code", args.Invite.Code);

                    await subscriber.DiscordOnInviteDeleted(sender, args);
                };
            }

            #endregion

            #region Message

            foreach (var service in messageEventsSubscriber)
            {
                Client.MessageCreated += async delegate (DiscordClient sender, MessageCreateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.MessageCreated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    if (args.Guild != null)
                        workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("Author.Id", args.Author.Id);
                    workScope.Span.SetTag("Message.Id", args.Message.Id);

                    await service.DiscordOnMessageCreated(sender, args);
                };

                Client.MessageAcknowledged += async delegate (DiscordClient sender, MessageAcknowledgeEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.MessageAcknowledged))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("Message.Id", args.Message.Id);

                    await service.DiscordOnMessageAcknowledged(sender, args);
                };

                Client.MessageUpdated += async delegate (DiscordClient sender, MessageUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.MessageUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    if (args.Guild != null)
                        workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("Author.Id", args.Author.Id);
                    workScope.Span.SetTag("Message.Id", args.Message.Id);

                    await service.DiscordOnMessageUpdated(sender, args);
                };

                Client.MessageDeleted += async delegate (DiscordClient sender, MessageDeleteEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.MessageDeleted))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    if (args.Guild != null)
                        workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("Message.Id", args.Message.Id);

                    await service.DiscordOnMessageDeleted(sender, args);
                };

                Client.MessagesBulkDeleted += async delegate (DiscordClient sender, MessageBulkDeleteEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.MessagesBulkDeleted))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    if (args.Guild != null)
                        workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);

                    await service.DiscordOnMessagesBulkDeleted(sender, args);
                };
            }

            #endregion

            #region Message Reaction

            foreach (var subscriber in messageReactionAddedEventsSubscriber)
            {
                Client.MessageReactionAdded += async delegate (DiscordClient sender, MessageReactionAddEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.MessageReactionAdded))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("User.Id", args.User.Id);
                    workScope.Span.SetTag("Message.Id", args.Message.Id);

                    await subscriber.DiscordOnMessageReactionAdded(sender, args);
                };

                Client.MessageReactionRemoved += async delegate (DiscordClient sender, MessageReactionRemoveEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.MessageReactionRemoved))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("User.Id", args.User.Id);
                    workScope.Span.SetTag("Message.Id", args.Message.Id);

                    await subscriber.DiscordOnMessageReactionRemoved(sender, args);
                };

                Client.MessageReactionsCleared += async delegate (DiscordClient sender, MessageReactionsClearEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.MessageReactionsCleared))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("Message.Id", args.Message.Id);

                    await subscriber.DiscordOnMessageReactionsCleared(sender, args);
                };

                Client.MessageReactionRemovedEmoji += async delegate (DiscordClient sender, MessageReactionRemoveEmojiEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.MessageReactionRemovedEmoji))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("Message.Id", args.Message.Id);
                    workScope.Span.SetTag("Emoji.Id", args.Emoji.Id);

                    await subscriber.DiscordOnMessageReactionRemovedEmoji(sender, args);
                };
            }

            #endregion

            #region Presence/User Update

            foreach (var subscriber in presenceUserEventsSubscriber)
            {
                Client.PresenceUpdated += async delegate (DiscordClient sender, PresenceUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.PresenceUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("User.Id", args.User.Id);

                    await subscriber.DiscordOnPresenceUpdated(sender, args);
                };

                Client.UserSettingsUpdated += async delegate (DiscordClient sender, UserSettingsUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.UserSettingsUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("User.Id", args.User.Id);

                    await subscriber.DiscordOnUserSettingsUpdated(sender, args);
                };

                Client.UserUpdated += async delegate (DiscordClient sender, UserUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.UserUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("UserBefore.Id", args.UserBefore.Id);
                    workScope.Span.SetTag("UserBefore.Id", args.UserBefore.Id);

                    await subscriber.DiscordOnUserUpdated(sender, args);
                };
            }

            #endregion

            #region Voice

            foreach (var subscriber in voiceEventsSubscriber)
            {
                Client.VoiceStateUpdated += async delegate (DiscordClient sender, VoiceStateUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.VoiceStateUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    if (args.Channel != null)
                        workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("User.Id", args.User.Id);

                    await subscriber.DiscordOnVoiceStateUpdated(sender, args);
                };

                Client.VoiceServerUpdated += async delegate (DiscordClient sender, VoiceServerUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.VoiceServerUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);

                    await subscriber.DiscordOnVoiceServerUpdated(sender, args);
                };
            }

            #endregion

            #region Misc

            foreach (var subscriber in miscEventsSubscriber)
                Client.ComponentInteractionCreated +=
                    async delegate (DiscordClient sender, ComponentInteractionCreateEventArgs args)
                    {
                        using var workScope = Tracer
                            .BuildSpan(nameof(Client.ComponentInteractionCreated))
                            .IgnoreActiveSpan()
                            .StartActive(true);
                        workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                        workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                        workScope.Span.SetTag("User.Id", args.User.Id);

                        await subscriber.DiscordOnComponentInteractionCreated(sender, args);
                    };

            #endregion
        }
    }
}