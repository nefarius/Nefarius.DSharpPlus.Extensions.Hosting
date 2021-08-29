using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordGuildMemberEventsSubscriber
    {
        /// <summary>
        ///     Fired when a new user joins a guild.
        ///     For this Event you need the <see cref="DiscordIntents.GuildMembers" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs args);

        /// <summary>
        ///     Fired when a user is removed from a guild (leave/kick/ban).
        ///     For this Event you need the <see cref="DiscordIntents.GuildMembers" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnGuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs args);

        /// <summary>
        ///     Fired when a guild member is updated.
        ///     For this Event you need the <see cref="DiscordIntents.GuildMembers" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnGuildMemberUpdated(DiscordClient sender, GuildMemberUpdateEventArgs args);

        /// <summary>
        ///     Fired in response to Gateway Request Guild Members.
        /// </summary>
        public Task DiscordOnGuildMembersChunked(DiscordClient sender, GuildMembersChunkEventArgs args);
    }
}