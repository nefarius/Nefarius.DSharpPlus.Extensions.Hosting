using System;
using DSharpPlus;

#region OPTIONAL CommandsNext integration
using DSharpPlus.CommandsNext;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting;
#endregion

#region OPTIONAL Interactivity integration
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using Nefarius.DSharpPlus.Interactivity.Extensions.Hosting;
#endregion

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nefarius.DSharpPlus.Extensions.Hosting;
using OpenTracing;
using OpenTracing.Mock;

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
                    // Tracer is required, if you don't use one, use the MockTracer
                    // 
                    services.AddSingleton<ITracer>(provider => new MockTracer());

                    //
                    // Adds DiscordClient singleton service you can use everywhere
                    // 
                    services.AddDiscord(options =>
                    {
                        //
                        // Minimum required configuration
                        // 
                        options.Configuration = new DiscordConfiguration
                        {
                            Token = "recommended to read bot token from configuration file"
                        };
                    });

                    #region OPTIONAL CommandsNext integration

                    services.AddDiscordCommandsNext(options =>
                    {
                        options.Configuration = new CommandsNextConfiguration
                        {
                            StringPrefixes = new[] {">"},

                            EnableDms = false,

                            EnableMentionPrefix = true
                        };
                    });

                    #endregion

                    #region OPTIONAL Interactivity integration

                    services.AddDiscordInteractivity(options =>
                    {
                        options.Configuration = new InteractivityConfiguration
                        {
                            PaginationBehaviour = PaginationBehaviour.WrapAround,

                            ResponseBehavior = InteractionResponseBehavior.Ack,

                            ResponseMessage = "That's not a valid button",

                            Timeout = TimeSpan.FromMinutes(2)
                        };
                    });

                    #endregion

                    //
                    // Register your module(s) for every events interface it implements
                    // 
                    services.AddDiscordGuildEventsSubscriber<BotModuleForGuildAndMemberEvents>();
                    services.AddDiscordGuildMemberEventsSubscriber<BotModuleForGuildAndMemberEvents>();

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