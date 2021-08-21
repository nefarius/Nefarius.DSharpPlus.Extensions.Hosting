using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordGuildRoleEventsSubscriber
    {
        public Task DiscordOnGuildRoleCreated(DiscordClient sender, GuildRoleCreateEventArgs args);

        public Task DiscordOnGuildRoleUpdated(DiscordClient sender, GuildRoleUpdateEventArgs args);

        public Task DiscordOnGuildRoleDeleted(DiscordClient sender, GuildRoleDeleteEventArgs args);
    }
}