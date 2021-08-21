using DSharpPlus;
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
                    });

                    services.AddDiscordGuildEventsSubscriber<GuildEventsSubscriberExample01>();
                    services.AddDiscordGuildMemberEventsSubscriber<GuildEventsSubscriberExample01>();
                    services.AddDiscordGuildEventsSubscriber<GuildEventsSubscriberExample02>();

                    services.AddDiscordHostedService();
                });
        }
    }
}