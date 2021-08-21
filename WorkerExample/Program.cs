using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
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
                    services.AddSingleton<ITracer>(provider => new MockTracer());

                    services.AddDiscord(options =>
                    {
                        options.Configuration = new DiscordConfiguration
                        {
                            Token = ""
                        };

                        options.Interactivity = new InteractivityConfiguration()
                        {
                            PaginationBehaviour = PaginationBehaviour.WrapAround,

                            ResponseBehavior = InteractionResponseBehavior.Ack,

                            ResponseMessage = "That's not a valid button",

                            Timeout = TimeSpan.FromMinutes(2)
                        };

                        options.CommandsNext = new CommandsNextConfiguration()
                        {
                            StringPrefixes = new[] { ">" },

                            EnableDms = false,

                            EnableMentionPrefix = true
                        };

                        options.RegisterCommands<AdminCommands>();
                    });

                    services.AddDiscordGuildEventsSubscriber<GuildEventsSubscriberExample01>();
                    services.AddDiscordGuildMemberEventsSubscriber<GuildEventsSubscriberExample01>();
                    services.AddDiscordGuildEventsSubscriber<GuildEventsSubscriberExample02>();

                    services.AddDiscordHostedService();
                });
        }
    }
}