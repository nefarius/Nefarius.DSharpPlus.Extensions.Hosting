using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordMessageEventsSubscriber
    {
        /// <summary>
        ///     Fired when a message is created.
        ///     For this Event you need the <see cref="DiscordIntents.GuildMessages" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnMessageCreated(DiscordClient sender, MessageCreateEventArgs args);

        /// <summary>
        ///     Fired when message is acknowledged by the user.
        ///     For this Event you need the <see cref="DiscordIntents.GuildMessages" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnMessageAcknowledged(DiscordClient sender, MessageAcknowledgeEventArgs args);

        /// <summary>
        ///     Fired when a message is updated.
        ///     For this Event you need the <see cref="DiscordIntents.GuildMessages" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnMessageUpdated(DiscordClient sender, MessageUpdateEventArgs args);

        /// <summary>
        ///     Fired when a message is deleted.
        ///     For this Event you need the <see cref="DiscordIntents.GuildMessages" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnMessageDeleted(DiscordClient sender, MessageDeleteEventArgs args);

        /// <summary>
        ///     Fired when multiple messages are deleted at once.
        ///     For this Event you need the <see cref="DiscordIntents.GuildMessages" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnMessagesBulkDeleted(DiscordClient sender, MessageBulkDeleteEventArgs args);
    }
}