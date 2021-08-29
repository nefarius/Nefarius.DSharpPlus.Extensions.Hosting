using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordGuildEventsSubscriber
    {
        /// <summary>
        ///     Fired when the user joins a new guild.
        ///     For this Event you need the <see cref="DiscordIntents.Guilds" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        /// <remarks>[alias="GuildJoined"][alias="JoinedGuild"]</remarks>
        public Task DiscordOnGuildCreated(DiscordClient sender, GuildCreateEventArgs args);

        /// <summary>
        ///     Fired when a guild is becoming available.
        ///     For this Event you need the <see cref="DiscordIntents.Guilds" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args);

        /// <summary>
        ///     Fired when a guild is updated.
        ///     For this Event you need the <see cref="DiscordIntents.Guilds" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnGuildUpdated(DiscordClient sender, GuildUpdateEventArgs args);

        /// <summary>
        ///     Fired when the user leaves or is removed from a guild.
        ///     For this Event you need the <see cref="DiscordIntents.Guilds" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnGuildDeleted(DiscordClient sender, GuildDeleteEventArgs args);

        /// <summary>
        ///     Fired when a guild becomes unavailable.
        /// </summary>
        public Task DiscordOnGuildUnavailable(DiscordClient sender, GuildDeleteEventArgs args);

        /// <summary>
        ///     Fired when all guilds finish streaming from Discord.
        /// </summary>
        public Task DiscordOnGuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs args);

        /// <summary>
        ///     Fired when a guilds emojis get updated
        ///     For this Event you need the <see cref="DiscordIntents.GuildEmojis" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnGuildEmojisUpdated(DiscordClient sender, GuildEmojisUpdateEventArgs args);

        public Task DiscordOnGuildStickersUpdated(DiscordClient sender, GuildStickersUpdateEventArgs args);

        /// <summary>
        ///     Fired when a guild integration is updated.
        /// </summary>
        public Task DiscordOnGuildIntegrationsUpdated(DiscordClient sender, GuildIntegrationsUpdateEventArgs args);
    }
}