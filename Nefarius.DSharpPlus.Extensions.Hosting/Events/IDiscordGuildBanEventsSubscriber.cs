using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordGuildBanEventsSubscriber
    {
        /// <summary>
        ///     Fired when a guild ban gets added
        ///     For this Event you need the <see cref="DiscordIntents.GuildBans" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnGuildBanAdded(DiscordClient sender, GuildBanAddEventArgs args);

        /// <summary>
        ///     Fired when a guild ban gets removed
        ///     For this Event you need the <see cref="DiscordIntents.GuildBans" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnGuildBanRemoved(DiscordClient sender, GuildBanRemoveEventArgs args);
    }
}