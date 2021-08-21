using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordGuildMemberEventsSubscriber
    {
        public Task DiscordOnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs args);

        public Task DiscordOnGuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs args);

        public Task DiscordOnGuildMemberUpdated(DiscordClient sender, GuildMemberUpdateEventArgs args);
    }
}