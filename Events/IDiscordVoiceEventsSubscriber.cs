using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordVoiceEventsSubscriber
    {
        public Task DiscordOnVoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs args);

        public Task DiscordOnVoiceServerUpdated(DiscordClient sender, VoiceServerUpdateEventArgs args);
    }
}