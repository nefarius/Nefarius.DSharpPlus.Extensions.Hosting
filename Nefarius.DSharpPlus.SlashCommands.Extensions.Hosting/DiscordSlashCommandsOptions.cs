using System;
using System.Collections.Generic;
using DSharpPlus.SlashCommands;
using JetBrains.Annotations;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting
{
    public class DiscordSlashCommandsOptions : IDiscordExtensionConfiguration
    {
        /// <summary>
        ///     Sets the <see cref="SlashCommandsConfiguration" /> to use.
        /// </summary>
        public SlashCommandsConfiguration Configuration { internal get; set; }

        /// <summary>
        ///     Gets the list of <see cref="Type" />s to register as command modules.
        /// </summary>
        internal IDictionary<Type, ulong?> CommandModules { get; } = new Dictionary<Type, ulong?>();

        /// <summary>
        ///     Registers a command class.
        /// </summary>
        /// <typeparam name="T">The command class to register.</typeparam>
        /// <param name="guildId">The guild id to register it on. If you want global commands, leave it null.</param>
        [UsedImplicitly]
        public void RegisterCommands<T>(ulong? guildId = null) where T : ApplicationCommandModule
        {
            CommandModules.Add(typeof(T), guildId);
        }
    }
}