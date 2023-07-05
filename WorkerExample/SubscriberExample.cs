using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace WorkerExample
{
    // this does the same as calling services.AddDiscordGuildAvailableEventSubscriber<BotModuleForGuildAndMemberEvents>();
    [DiscordGuildAvailableEventSubscriber]
    // this does the same as calling services.AddDiscordGuildMemberAddedEventSubscriber<BotModuleForGuildAndMemberEvents>();
    [DiscordGuildMemberAddedEventSubscriber]
    internal class BotModuleForGuildAndMemberEvents :
        // you can implement one or many interfaces for event handlers in one class or split it however you like. Your choice!
        IDiscordGuildAvailableEventSubscriber,
        IDiscordGuildMemberAddedEventSubscriber
    {
        private readonly ILogger<BotModuleForGuildAndMemberEvents> _logger;

        /// <summary>
        ///     Optional constructor for Dependency Injection. parameters get populated automatically with you services.
        /// </summary>
        /// <param name="logger">The logger service instance.</param>
        /// <param name="tracer">The tracer service instance.</param>
        public BotModuleForGuildAndMemberEvents(
            ILogger<BotModuleForGuildAndMemberEvents> logger
        )
        {
            //
            // Do whatever you like with these. It's recommended to not do heavy tasks in 
            // constructors, just store your service references for later use!
            // 
            // You can inject scoped services like database contexts as well!
            // 
            _logger = logger;
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

        public Task DiscordOnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs args)
        {
            //
            // Fired when a new member has joined, exciting!
            // 
            _logger.LogInformation("New member {Member} joined!", args.Member);

            //
            // Return successful execution
            // 
            return Task.CompletedTask;
        }
    }

    // no attributes, so services.AddDiscordXXXEventsSubscriber<BotModuleForMiscEvents>(); needs to be called manually!
    internal class BotModuleForMiscEvents : 
        IDiscordComponentInteractionCreatedEventSubscriber,
        IDiscordClientErroredEventSubscriber
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