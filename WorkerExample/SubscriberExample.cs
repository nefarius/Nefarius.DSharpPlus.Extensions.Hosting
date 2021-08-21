using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace WorkerExample
{
    internal interface IGuildEventsSubscriberExample : IDiscordGuildEventsSubscriber
    {
    }

    internal class GuildEventsSubscriberExample01 : IGuildEventsSubscriberExample
    {
        public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildUpdated(DiscordClient sender, GuildUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }
    }

    internal class GuildEventsSubscriberExample02 : IGuildEventsSubscriberExample
    {
        public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildUpdated(DiscordClient sender, GuildUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
