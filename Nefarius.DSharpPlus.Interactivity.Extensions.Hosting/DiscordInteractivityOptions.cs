using DSharpPlus.Interactivity;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.Interactivity.Extensions.Hosting
{
    public class DiscordInteractivityOptions : IDiscordExtensionConfiguration
    {
        /// <summary>
        ///     Sets the <see cref="InteractivityConfiguration" /> to use.
        /// </summary>
        public InteractivityConfiguration Configuration { internal get; set; }
    }
}
