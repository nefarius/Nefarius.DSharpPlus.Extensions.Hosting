using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordVoiceEventsSubscriber
    {
        /// <summary>
        ///     Fired when someone joins/leaves/moves voice channels.
        ///     For this Event you need the <see cref="DiscordIntents.GuildVoiceStates" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnVoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs args);

        /// <summary>
        ///     Fired when a guild's voice server is updated.
        ///     For this Event you need the <see cref="DiscordIntents.GuildVoiceStates" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnVoiceServerUpdated(DiscordClient sender, VoiceServerUpdateEventArgs args);
    }
}