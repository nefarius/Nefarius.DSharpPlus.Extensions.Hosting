using System;
using Microsoft.Extensions.Hosting;
using Nefarius.DSharpPlus.Extensions.Hosting;

#region OPTIONAL CommandsNext integration

using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting;

#endregion

#region OPTIONAL Interactivity integration

using DSharpPlus.Interactivity.Enums;
using Nefarius.DSharpPlus.Interactivity.Extensions.Hosting;

#endregion

namespace WorkerExample;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                //
                // Adds DiscordClient singleton service you can use everywhere
                // 
                services.AddDiscord(options =>
                    {
                        //
                        // Minimum required configuration
                        // 
                        options.Token = "recommended to read bot token from configuration file";
                    },
                    // optional parameters for ConnectAsync()
                    connectOptions: copts => { copts.IdleSince = DateTimeOffset.UtcNow; });

                #region OPTIONAL CommandsNext integration

                services.AddDiscordCommandsNext(options =>
                {
                    options.StringPrefixes = new[] { ">" };
                    options.EnableDms = false;
                    options.EnableMentionPrefix = true;
                }, extension =>
                {
                    extension.RegisterCommands<AdminCommands>();
                    extension.RegisterCommands<MemberCommands>();
                });

                #endregion

                #region OPTIONAL Interactivity integration

                services.AddDiscordInteractivity(options =>
                {
                    options.PaginationBehaviour = PaginationBehaviour.WrapAround;
                    options.ResponseBehavior = InteractionResponseBehavior.Ack;
                    options.ResponseMessage = "That's not a valid button";
                    options.Timeout = TimeSpan.FromMinutes(2);
                });

                #endregion

                //
                // Register your module(s) for every events interface it implements.
                // Commented out because the attributes on the subscriber classes
                // do the same for you automatically. Isn't that nice of them? :)
                // 
                /*
                services.AddDiscordGuildAvailableEventSubscriber<BotModuleForGuildAndMemberEvents>();
                services.AddDiscordGuildMemberAddedEventSubscriber<BotModuleForGuildAndMemberEvents>();
                */

                //
                // Module reacting to different events
                // 
                services.AddDiscordComponentInteractionCreatedEventSubscriber<BotModuleForMiscEvents>();
                services.AddDiscordClientErroredEventSubscriber<BotModuleForMiscEvents>();

                //
                // Automatically host service and connect to gateway on boot
                // 
                services.AddDiscordHostedService();
            });
    }
}