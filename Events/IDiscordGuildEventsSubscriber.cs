using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordGuildEventsSubscriber
    {
        public Task DiscordOnGuildCreated(DiscordClient sender, GuildCreateEventArgs args);

        public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args);

        public Task DiscordOnGuildUpdated(DiscordClient sender, GuildUpdateEventArgs args);

        public Task DiscordOnGuildDeleted(DiscordClient sender, GuildDeleteEventArgs args);

        public Task DiscordOnGuildUnavailable(DiscordClient sender, GuildDeleteEventArgs args);

        public Task DiscordOnGuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs args);

        public Task DiscordOnGuildEmojisUpdated(DiscordClient sender, GuildEmojisUpdateEventArgs args);

        public Task DiscordOnGuildStickersUpdated(DiscordClient sender, GuildStickersUpdateEventArgs args);

        public Task DiscordOnGuildIntegrationsUpdated(DiscordClient sender, GuildIntegrationsUpdateEventArgs args);
    }
}