using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace WorkerExample
{

    internal class BotModuleForGuildAndMemberEvents : 
        IDiscordGuildEventsSubscriber,
        IDiscordGuildMemberEventsSubscriber
    {
        public Task DiscordOnGuildCreated(DiscordClient sender, GuildCreateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args)
        {
            Console.WriteLine(args.Guild.Name);

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
            return Task.CompletedTask;
        }

        public Task DiscordOnGuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs args)
        {
            return Task.CompletedTask;
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

    internal class BotModuleForMiscEvents : IDiscordMiscEventsSubscriber
    {
        public async Task DiscordOnComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            throw new NotImplementedException();
        }

        public async Task DiscordOnClientErrored(DiscordClient sender, ClientErrorEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}