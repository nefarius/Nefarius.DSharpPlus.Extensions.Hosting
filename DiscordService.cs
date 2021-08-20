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

        public void RegisterGuildEventsSubscriber<T>() where T : IDiscordGuildEventsSubscriber;

        public void RegisterGuildMemberEventsSubscriber<T>() where T : IDiscordGuildMemberEventsSubscriber;

        public void RegisterInviteEventsSubscriber<T>() where T : IDiscordInviteEventsSubscriber;

        public void RegisterMessageEventsSubscriber<T>() where T : IDiscordMessageEventsSubscriber;

        public void RegisterVoiceEventsSubscriber<T>() where T : IDiscordVoiceEventsSubscriber;

        public void RegisterComponentInteractionEventsSubscriber<T>()
            where T : IDiscordComponentInteractionEventsSubscriber;
    }

    [UsedImplicitly]
    public class DiscordService : IDiscordClientService
    {
        protected readonly ILogger<DiscordService> Logger;

        protected readonly IServiceProvider ServiceProvider;

        protected readonly ITracer Tracer;

        protected readonly IOptions<DiscordConfiguration> Options;

        public DiscordService(
            IServiceProvider serviceProvider,
            ILoggerFactory logFactory,
            ILogger<DiscordService> logger,
            ITracer tracer,
            IOptions<DiscordConfiguration> options
        )
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
            Tracer = tracer;
            Options = options;

            if (Options.Value == null)
                throw new InvalidOperationException($"{nameof(DiscordConfiguration)} is required");

            Options.Value.LoggerFactory = logFactory;

            Client = new DiscordClient(Options.Value);
        }

        public DiscordClient Client { get; }

        public void RegisterGuildEventsSubscriber<T>() where T : IDiscordGuildEventsSubscriber
        {
            Client.GuildAvailable += async delegate (DiscordClient sender, GuildCreateEventArgs args)
            {
                using var workScope = Tracer
                    .BuildSpan(nameof(Client.GuildAvailable))
                    .IgnoreActiveSpan()
                    .StartActive(true);
                workScope.Span.SetTag("Guild.Id", args.Guild.Id);

                using var scope = ServiceProvider.CreateScope();

                await scope.ServiceProvider.GetRequiredService<T>().DiscordOnGuildAvailable(sender, args);
            };

            Client.GuildUpdated += async delegate (DiscordClient sender, GuildUpdateEventArgs args)
            {
                using var workScope = Tracer
                    .BuildSpan(nameof(Client.GuildAvailable))
                    .IgnoreActiveSpan()
                    .StartActive(true);
                workScope.Span.SetTag("Guild.Id", args.GuildBefore.Id);

                using var scope = ServiceProvider.CreateScope();

                await scope.ServiceProvider.GetRequiredService<T>().DiscordOnGuildUpdated(sender, args);
            };
        }

        public void RegisterGuildMemberEventsSubscriber<T>() where T : IDiscordGuildMemberEventsSubscriber
        {
            Client.GuildMemberAdded += async delegate (DiscordClient sender, GuildMemberAddEventArgs args)
            {
                using var workScope = Tracer
                    .BuildSpan(nameof(Client.GuildMemberAdded))
                    .IgnoreActiveSpan()
                    .StartActive(true);
                workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                workScope.Span.SetTag("Member.Id", args.Member.Id);

                using var scope = ServiceProvider.CreateScope();

                await scope.ServiceProvider.GetRequiredService<T>().DiscordOnGuildMemberAdded(sender, args);
            };

            Client.GuildMemberUpdated += async delegate (DiscordClient sender, GuildMemberUpdateEventArgs args)
            {
                using var workScope = Tracer
                    .BuildSpan(nameof(Client.GuildMemberUpdated))
                    .IgnoreActiveSpan()
                    .StartActive(true);
                workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                workScope.Span.SetTag("Member.Id", args.Member.Id);

                using var scope = ServiceProvider.CreateScope();

                await scope.ServiceProvider.GetRequiredService<T>().DiscordOnGuildMemberUpdated(sender, args);
            };

            Client.GuildMemberRemoved += async delegate (DiscordClient sender, GuildMemberRemoveEventArgs args)
            {
                using var workScope = Tracer
                    .BuildSpan(nameof(Client.GuildMemberRemoved))
                    .IgnoreActiveSpan()
                    .StartActive(true);
                workScope.Span.SetTag("Guild.Id", args.Guild.Id);
                workScope.Span.SetTag("Member.Id", args.Member.Id);

                using var scope = ServiceProvider.CreateScope();

                await scope.ServiceProvider.GetRequiredService<T>().DiscordOnGuildMemberRemoved(sender, args);
            };
        }

        public void RegisterInviteEventsSubscriber<T>() where T : IDiscordInviteEventsSubscriber
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

                using var scope = ServiceProvider.CreateScope();

                await scope.ServiceProvider.GetRequiredService<T>().DiscordOnInviteCreated(sender, args);
            };
        }

        public void RegisterMessageEventsSubscriber<T>() where T : IDiscordMessageEventsSubscriber
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

                using var scope = ServiceProvider.CreateScope();

                await scope.ServiceProvider.GetRequiredService<T>().DiscordOnMessageCreated(sender, args);
            };
        }

        public void RegisterVoiceEventsSubscriber<T>() where T : IDiscordVoiceEventsSubscriber
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

                using var scope = ServiceProvider.CreateScope();

                await scope.ServiceProvider.GetRequiredService<T>().DiscordOnVoiceStateUpdated(sender, args);
            };
        }

        public void RegisterComponentInteractionEventsSubscriber<T>()
            where T : IDiscordComponentInteractionEventsSubscriber
        {
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

                    using var scope = ServiceProvider.CreateScope();

                    await scope.ServiceProvider.GetRequiredService<T>()
                        .DiscordOnComponentInteractionCreated(sender, args);
                };
        }
    }
}