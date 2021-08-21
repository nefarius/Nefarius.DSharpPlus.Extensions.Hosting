using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace WorkerExample
{
    internal interface IGuildEventsSubscriberExample : IDiscordGuildEventsSubscriber,
        IDiscordGuildMemberEventsSubscriber
    {
    }

    internal class GuildEventsSubscriberExample01 : IGuildEventsSubscriberExample, IDisposable
    {
        public void Dispose()
        {
        }

        public Task DiscordOnGuildCreated(DiscordClient sender, GuildCreateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildUpdated(DiscordClient sender, GuildUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildDeleted(DiscordClient sender, GuildDeleteEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildUnavailable(DiscordClient sender, GuildDeleteEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildEmojisUpdated(DiscordClient sender, GuildEmojisUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildStickersUpdated(DiscordClient sender, GuildStickersUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildIntegrationsUpdated(DiscordClient sender, GuildIntegrationsUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildMemberUpdated(DiscordClient sender, GuildMemberUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }
    }

    internal class GuildEventsSubscriberExample02 : IGuildEventsSubscriberExample
    {
        public Task DiscordOnGuildCreated(DiscordClient sender, GuildCreateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args)
        {
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildUpdated(DiscordClient sender, GuildUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildDeleted(DiscordClient sender, GuildDeleteEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildUnavailable(DiscordClient sender, GuildDeleteEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildEmojisUpdated(DiscordClient sender, GuildEmojisUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildStickersUpdated(DiscordClient sender, GuildStickersUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildIntegrationsUpdated(DiscordClient sender, GuildIntegrationsUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildMemberUpdated(DiscordClient sender, GuildMemberUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}