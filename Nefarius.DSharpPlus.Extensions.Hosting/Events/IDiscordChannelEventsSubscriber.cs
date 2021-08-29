using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordChannelEventsSubscriber
    {
        /// <summary>
        ///     Fired when a new channel is created.
        ///     For this Event you need the <see cref="DiscordIntents.Guilds" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnChannelCreated(DiscordClient sender, ChannelCreateEventArgs args);

        /// <summary>
        ///     Fired when a channel is updated.
        ///     For this Event you need the <see cref="DiscordIntents.Guilds" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnChannelUpdated(DiscordClient sender, ChannelUpdateEventArgs args);

        /// <summary>
        ///     Fired when a channel is deleted
        ///     For this Event you need the <see cref="DiscordIntents.Guilds" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnChannelDeleted(DiscordClient sender, ChannelDeleteEventArgs args);

        /// <summary>
        ///     Fired when a dm channel is deleted
        ///     For this Event you need the <see cref="DiscordIntents.DirectMessages" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnDmChannelDeleted(DiscordClient sender, DmChannelDeleteEventArgs args);

        /// <summary>
        ///     Fired when a dm channel is deleted
        ///     For this Event you need the <see cref="DiscordIntents.DirectMessages" /> intent specified in
        ///     <seealso cref="DiscordConfiguration.Intents" />
        /// </summary>
        public Task DiscordOnChannelPinsUpdated(DiscordClient sender, ChannelPinsUpdateEventArgs args);
    }
}