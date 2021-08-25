using DSharpPlus.VoiceNext;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.VoiceNext.Extensions.Hosting
{
    public class DiscordVoiceNextOptions : IDiscordExtensionConfiguration
    {
        /// <summary>
        ///     Sets the <see cref="VoiceNextConfiguration" /> to use.
        /// </summary>
        public VoiceNextConfiguration Configuration { internal get; set; }
    }
}
