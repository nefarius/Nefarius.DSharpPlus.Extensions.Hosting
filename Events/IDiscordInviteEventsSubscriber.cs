using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordInviteEventsSubscriber
    {
        public Task DiscordOnInviteCreated(DiscordClient sender, InviteCreateEventArgs args);

        public Task DiscordOnInviteDeleted(DiscordClient sender, InviteDeleteEventArgs args);
    }
}