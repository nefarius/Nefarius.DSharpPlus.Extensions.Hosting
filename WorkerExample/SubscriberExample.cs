using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using Nefarius.DSharpPlus.Extensions.Hosting.Attributes;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;
using OpenTracing;

namespace WorkerExample
{
    [DiscordGuildEventsSubscriber]
    [DiscordGuildMemberEventsSubscriber]
    internal class BotModuleForGuildAndMemberEvents : 
        IDiscordGuildEventsSubscriber,
        IDiscordGuildMemberEventsSubscriber
    {
        private readonly ILogger<BotModuleForGuildAndMemberEvents> _logger;

        private readonly ITracer _tracer;

        public BotModuleForGuildAndMemberEvents(
            ILogger<BotModuleForGuildAndMemberEvents> logger,
            ITracer tracer
        )
        {
            _logger = logger;
            _tracer = tracer;
        }

        public Task DiscordOnGuildCreated(DiscordClient sender, GuildCreateEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args)
        {
            //
            // To see some action, output the guild name
            // 
            Console.WriteLine(args.Guild.Name);

            return Task.CompletedTask;
        }

        public Task DiscordOnGuildUpdated(DiscordClient sender, GuildUpdateEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildDeleted(DiscordClient sender, GuildDeleteEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildUnavailable(DiscordClient sender, GuildDeleteEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildEmojisUpdated(DiscordClient sender, GuildEmojisUpdateEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildStickersUpdated(DiscordClient sender, GuildStickersUpdateEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildIntegrationsUpdated(DiscordClient sender, GuildIntegrationsUpdateEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildMemberUpdated(DiscordClient sender, GuildMemberUpdateEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildMembersChunked(DiscordClient sender, GuildMembersChunkEventArgs args)
        {
            return Task.CompletedTask;
        }
    }

    internal class BotModuleForMiscEvents : IDiscordMiscEventsSubscriber
    {
        public Task DiscordOnComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnClientErrored(DiscordClient sender, ClientErrorEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}