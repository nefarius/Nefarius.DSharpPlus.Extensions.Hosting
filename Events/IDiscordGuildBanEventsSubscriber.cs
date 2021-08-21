using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordGuildBanEventsSubscriber
    {
        public Task DiscordOnGuildBanAdded(DiscordClient sender, GuildBanAddEventArgs args);

        public Task DiscordOnGuildBanRemoved(DiscordClient sender, GuildBanRemoveEventArgs args);
    }
}