using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordWebSocketEventSubscriber
    {
        public Task DiscordOnReady(DiscordClient sender, ReadyEventArgs args);
    }
}
