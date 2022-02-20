using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{
    public interface IDiscordWebSocketEventSubscriber
    {
        /// <summary>
        ///     Fired whenever a WebSocket error occurs within the client.
        /// </summary>
        public Task DiscordOnSocketErrored(DiscordClient sender, SocketErrorEventArgs args);

        /// <summary>
        ///     Fired whenever WebSocket connection is established.
        /// </summary>
        public Task DiscordOnSocketOpened(DiscordClient sender, SocketEventArgs args);

        /// <summary>
        ///     Fired whenever WebSocket connection is terminated.
        /// </summary>
        public Task DiscordOnSocketClosed(DiscordClient sender, SocketCloseEventArgs args);

        /// <summary>
        ///     Fired when the client enters ready state.
        /// </summary>
        public Task DiscordOnReady(DiscordClient sender, ReadyEventArgs args);

        /// <summary>
        ///     Fired whenever a session is resumed.
        /// </summary>
        public Task DiscordOnResumed(DiscordClient sender, ReadyEventArgs args);

        /// <summary>
        ///     Fired on received heartbeat ACK.
        /// </summary>
        public Task DiscordOnHeartbeated(DiscordClient sender, HeartbeatEventArgs args);

        /// <summary>
        ///     Fired on heartbeat attempt cancellation due to too many failed heartbeats.
        /// </summary>
        public Task DiscordOnZombied(DiscordClient sender, ZombiedEventArgs args);
    }
}