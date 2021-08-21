using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordChannelEventsSubscriber
    {
        public Task DiscordOnChannelCreated(DiscordClient sender, ChannelCreateEventArgs args);

        public Task DiscordOnChannelUpdated(DiscordClient sender, ChannelUpdateEventArgs args);

        public Task DiscordOnChannelDeleted(DiscordClient sender, ChannelDeleteEventArgs args);

        public Task DiscordOnDmChannelDeleted(DiscordClient sender, DmChannelDeleteEventArgs args);

        public Task DiscordOnChannelPinsUpdated(DiscordClient sender, ChannelPinsUpdateEventArgs args);
    }
}