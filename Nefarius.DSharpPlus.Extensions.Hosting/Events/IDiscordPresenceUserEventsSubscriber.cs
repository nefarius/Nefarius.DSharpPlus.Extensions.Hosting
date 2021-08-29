using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordPresenceUserEventsSubscriber
    {
        /// <summary>
        ///     Fired when a presence has been updated.
        ///     For this Event you need the <see cref="DiscordIntents.GuildPresences" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnPresenceUpdated(DiscordClient sender, PresenceUpdateEventArgs args);

        /// <summary>
        ///     Fired when the current user updates their settings.
        ///     For this Event you need the <see cref="DiscordIntents.GuildPresences" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnUserSettingsUpdated(DiscordClient sender, UserSettingsUpdateEventArgs args);

        /// <summary>
        ///     Fired when properties about the current user change.
        /// </summary>
        /// <remarks>
        ///     NB: This event only applies for changes to the <b>current user</b>, the client that is connected to Discord.
        ///     For this Event you need the <see cref="DiscordIntents.GuildPresences" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </remarks>
        public Task DiscordOnUserUpdated(DiscordClient sender, UserUpdateEventArgs args);
    }
}