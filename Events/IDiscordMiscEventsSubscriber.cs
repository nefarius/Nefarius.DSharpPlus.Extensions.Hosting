using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordMiscEventsSubscriber
    {
        public Task DiscordOnComponentInteractionCreated(DiscordClient sender,
            ComponentInteractionCreateEventArgs args);
    }
}