using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordInviteEventsSubscriber
    {
        /// <summary>
        ///     Fired when an invite is created.
        ///     For this Event you need the <see cref="DiscordIntents.GuildInvites" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnInviteCreated(DiscordClient sender, InviteCreateEventArgs args);

        /// <summary>
        ///     Fired when an invite is deleted.
        ///     For this Event you need the <see cref="DiscordIntents.GuildInvites" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnInviteDeleted(DiscordClient sender, InviteDeleteEventArgs args);
    }
}