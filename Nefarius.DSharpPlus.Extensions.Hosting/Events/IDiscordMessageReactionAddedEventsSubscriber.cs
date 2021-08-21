using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordMessageReactionAddedEventsSubscriber
    {
        public Task DiscordOnMessageReactionAdded(DiscordClient sender, MessageReactionAddEventArgs args);

        public Task DiscordOnMessageReactionRemoved(DiscordClient sender, MessageReactionRemoveEventArgs args);

        public Task DiscordOnMessageReactionsCleared(DiscordClient sender, MessageReactionsClearEventArgs args);

        public Task DiscordOnMessageReactionRemovedEmoji(DiscordClient sender,
            MessageReactionRemoveEmojiEventArgs args);
    }
}