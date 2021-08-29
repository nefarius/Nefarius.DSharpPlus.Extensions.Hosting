using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordMiscEventsSubscriber
    {
        /// <summary>
        ///     Fired when a component is invoked.
        /// </summary>
        public Task DiscordOnComponentInteractionCreated(DiscordClient sender,
            ComponentInteractionCreateEventArgs args);

        /// <summary>
        ///     Fired whenever an error occurs within an event handler.
        /// </summary>
        public Task DiscordOnClientErrored(DiscordClient sender, ClientErrorEventArgs args);
    }
}