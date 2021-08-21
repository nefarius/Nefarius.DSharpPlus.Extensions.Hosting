using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordPresenceUserEventsSubscriber
    {
        public Task DiscordOnPresenceUpdated(DiscordClient sender, PresenceUpdateEventArgs args);

        public Task DiscordOnUserSettingsUpdated(DiscordClient sender, UserSettingsUpdateEventArgs args);

        public Task DiscordOnUserUpdated(DiscordClient sender, UserUpdateEventArgs args);
    }
}