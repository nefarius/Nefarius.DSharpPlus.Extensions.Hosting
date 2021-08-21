using DSharpPlus;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    /// <summary>
    ///     Configuration to bootstrap the <see cref="IDiscordClientService" />.
    /// </summary>
    public class DiscordServiceOptions
    {
        /// <summary>
        ///     Sets the <see cref="DiscordConfiguration"/> to use.
        /// </summary>
        public DiscordConfiguration Configuration { internal get; set; }
    }
}