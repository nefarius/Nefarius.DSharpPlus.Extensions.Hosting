using System;
using System.Collections.Generic;
using DSharpPlus.CommandsNext;
using JetBrains.Annotations;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting
{
    public class DiscordCommandsNextOptions : IDiscordExtensionConfiguration
    {
        /// <summary>
        ///     Sets the <see cref="CommandsNextConfiguration" /> to use.
        /// </summary>
        public CommandsNextConfiguration Configuration { internal get; set; }

        /// <summary>
        ///     Gets the list of <see cref="Type" />s to register as command modules.
        /// </summary>
        internal IList<Type> CommandModules { get; } = new List<Type>();

        /// <summary>
        ///     Registers all commands from a given command class.
        /// </summary>
        /// <typeparam name="T">Class which holds commands to register.</typeparam>
        [UsedImplicitly]
        public void RegisterCommands<T>() where T : BaseCommandModule
        {
            CommandModules.Add(typeof(T));
        }
    }
}
