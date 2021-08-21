using System;
using DSharpPlus;
using DSharpPlus.EventArgs;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            var configuration = new DiscordConfiguration(Options.Value.Configuration) {LoggerFactory = LogFactory};

            Client = new DiscordClient(configuration);

            using var scope = ServiceProvider.CreateScope();

            foreach (IDiscordGuildEventsSubscriber service in scope.ServiceProvider.GetServices(
                typeof(IDiscordGuildEventsSubscriber)))
            {
                Client.GuildAvailable += async delegate(DiscordClient sender, GuildCreateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildAvailable))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);

                    await service.DiscordOnGuildAvailable(sender, args);
                };

                Client.GuildUpdated += async delegate(DiscordClient sender, GuildUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildAvailable))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.GuildBefore.Id);

                    await service.DiscordOnGuildUpdated(sender, args);
                };
            }

            foreach (IDiscordGuildMemberEventsSubscriber service in scope.ServiceProvider.GetServices(
                typeof(IDiscordGuildMemberEventsSubscriber)))
            {
                Client.GuildMemberAdded += async delegate(DiscordClient sender, GuildMemberAddEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildMemberAdded))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Member.Id", args.Member.Id);

                    await service.DiscordOnGuildMemberAdded(sender, args);
                };

                Client.GuildMemberUpdated += async delegate(DiscordClient sender, GuildMemberUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildMemberUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Member.Id", args.Member.Id);

                    await service.DiscordOnGuildMemberUpdated(sender, args);
                };

                Client.GuildMemberRemoved += async delegate(DiscordClient sender, GuildMemberRemoveEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.GuildMemberRemoved))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Member.Id", args.Member.Id);

                    await service.DiscordOnGuildMemberRemoved(sender, args);
                };
            }

            foreach (IDiscordInviteEventsSubscriber service in scope.ServiceProvider.GetServices(
                typeof(IDiscordInviteEventsSubscriber)))
                Client.InviteCreated += async delegate(DiscordClient sender, InviteCreateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.InviteCreated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("Invite.Code", args.Invite.Code);

                    await service.DiscordOnInviteCreated(sender, args);
                };

            foreach (IDiscordMessageEventsSubscriber service in scope.ServiceProvider.GetServices(
                typeof(IDiscordMessageEventsSubscriber)))
                Client.MessageCreated += async delegate(DiscordClient sender, MessageCreateEventArgs args)
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

            foreach (IDiscordVoiceEventsSubscriber service in scope.ServiceProvider.GetServices(
                typeof(IDiscordVoiceEventsSubscriber)))
                Client.VoiceStateUpdated += async delegate(DiscordClient sender, VoiceStateUpdateEventArgs args)
                {
                    using var workScope = Tracer
                        .BuildSpan(nameof(Client.VoiceStateUpdated))
                        .IgnoreActiveSpan()
                        .StartActive(true);
                    workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                    if (args.Channel != null)
                        workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                    workScope.Span.SetTag("User.Id", args.User.Id);

                    await service.DiscordOnVoiceStateUpdated(sender, args);
                };

            foreach (IDiscordComponentInteractionEventsSubscriber service in scope.ServiceProvider.GetServices(
                typeof(IDiscordComponentInteractionEventsSubscriber)))
                Client.ComponentInteractionCreated +=
                    async delegate(DiscordClient sender, ComponentInteractionCreateEventArgs args)
                    {
                        using var workScope = Tracer
                            .BuildSpan(nameof(Client.ComponentInteractionCreated))
                            .IgnoreActiveSpan()
                            .StartActive(true);
                        workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                        workScope.Span.SetTag("Channel.Id", args.Channel.Id);
                        workScope.Span.SetTag("User.Id", args.User.Id);

                        await service.DiscordOnComponentInteractionCreated(sender, args);
                    };
        }
    }
}