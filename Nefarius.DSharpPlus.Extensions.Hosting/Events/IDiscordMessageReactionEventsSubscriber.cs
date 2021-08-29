using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordMessageReactionEventsSubscriber
    {
        /// <summary>
        ///     Fired when a reaction gets added to a message.
        ///     For this Event you need the <see cref="DiscordIntents.GuildMessageReactions" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnMessageReactionAdded(DiscordClient sender, MessageReactionAddEventArgs args);

        /// <summary>
        ///     Fired when a reaction gets removed from a message.
        ///     For this Event you need the <see cref="DiscordIntents.GuildMessageReactions" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnMessageReactionRemoved(DiscordClient sender, MessageReactionRemoveEventArgs args);

        /// <summary>
        ///     Fired when all reactions get removed from a message.
        ///     For this Event you need the <see cref="DiscordIntents.GuildMessageReactions" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnMessageReactionsCleared(DiscordClient sender, MessageReactionsClearEventArgs args);

        /// <summary>
        ///     Fired when all reactions of a specific reaction are removed from a message.
        ///     For this Event you need the <see cref="DiscordIntents.GuildMessageReactions" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnMessageReactionRemovedEmoji(DiscordClient sender,
            MessageReactionRemoveEmojiEventArgs args);
    }
}