using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordMessageEventsSubscriber
    {
        public Task DiscordOnMessageCreated(DiscordClient sender, MessageCreateEventArgs args);

        public Task DiscordOnMessageAcknowledged(DiscordClient sender, MessageAcknowledgeEventArgs args);

        public Task DiscordOnMessageUpdated(DiscordClient sender, MessageUpdateEventArgs args);

        public Task DiscordOnMessageDeleted(DiscordClient sender, MessageDeleteEventArgs args);

        public Task DiscordOnMessagesBulkDeleted(DiscordClient sender, MessageBulkDeleteEventArgs args);
    }
}