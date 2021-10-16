using System;

#region OPTIONAL CommandsNext integration
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting;
#endregion

#region OPTIONAL Interactivity integration
using DSharpPlus.Interactivity.Enums;
using Nefarius.DSharpPlus.Interactivity.Extensions.Hosting;
#endregion

using Microsoft.Extensions.Hosting;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace WorkerExample
{
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
                    });

                    #region OPTIONAL CommandsNext integration

                    services.AddDiscordCommandsNext(options =>
                    {
                        options.StringPrefixes = new[] {">"};
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
                    // Register your module(s) for every events interface it implements
                    // 
                    /*
                    services.AddDiscordGuildEventsSubscriber<BotModuleForGuildAndMemberEvents>();
                    services.AddDiscordGuildMemberEventsSubscriber<BotModuleForGuildAndMemberEvents>();
                    */

                    //
                    // Module reacting to different events
                    // 
                    services.AddDiscordMiscEventsSubscriber<BotModuleForMiscEvents>();

                    //
                    // Automatically host service and connect to gateway on boot
                    // 
                    services.AddDiscordHostedService();
                });
        }
    }
}