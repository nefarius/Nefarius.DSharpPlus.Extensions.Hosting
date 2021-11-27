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
    // this does the same as calling services.AddDiscordGuildEventsSubscriber<BotModuleForGuildAndMemberEvents>();
    [DiscordGuildEventsSubscriber]
    // this does the same as calling services.AddDiscordGuildMemberEventsSubscriber<BotModuleForGuildAndMemberEvents>();
    [DiscordGuildMemberEventsSubscriber]
    internal class BotModuleForGuildAndMemberEvents :
        // you can implement one or many interfaces for event handlers in one class or split it however you like. Your choice!
        IDiscordGuildEventsSubscriber,
        IDiscordGuildMemberEventsSubscriber
    {
        private readonly ILogger<BotModuleForGuildAndMemberEvents> _logger;

        private readonly ITracer _tracer;

        /// <summary>
        ///     Optional constructor for Dependency Injection. parameters get populated automatically with you services.
        /// </summary>
        /// <param name="logger">The logger service instance.</param>
        /// <param name="tracer">The tracer service instance.</param>
        public BotModuleForGuildAndMemberEvents(
            ILogger<BotModuleForGuildAndMemberEvents> logger,
            ITracer tracer
        )
        {
            //
            // Do whatever you like with these. It's recommended to not do heavy tasks in 
            // constructors, just store your service references for later use!
            // 
            // You can inject scoped services like database contexts as well!
            // 
            _logger = logger;
            _tracer = tracer;
        }

        public Task DiscordOnGuildCreated(DiscordClient sender, GuildCreateEventArgs args)
        {
            //
            // You are not interested in this event? Just return a Task.CompletedTask:
            // 
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args)
        {
            //
            // To see some action, output the guild name
            // 
            Console.WriteLine(args.Guild.Name);

            //
            // Usage of injected logger service
            // 
            _logger.LogInformation("Guild {Guild} came online", args.Guild);

            //
            // Return successful execution
            // 
            return Task.CompletedTask;
        }

        //
        // It might get a bit cluttered when you're only interested in a few events.
        // You can #region and hide them or make this class partial. Thinking of a
        // less verbose and crude solution in a future design. Maybe :)
        // 

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

    // no attributes, so services.AddDiscordMiscEventsSubscriber<BotModuleForMiscEvents>(); needs to be called manually!
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