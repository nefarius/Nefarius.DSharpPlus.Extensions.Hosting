using System;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using JetBrains.Annotations;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    public class DiscordServiceOptions
    {
        public DiscordConfiguration Configuration { get; set; } = new DiscordConfiguration();

        public InteractivityConfiguration Interactivity { get; set; }

        public CommandsNextConfiguration CommandsNext { get; set; }

        public IList<Type> CommandModules { get; } = new List<Type>();

        [UsedImplicitly]
        public void RegisterCommands<T>() where T : BaseCommandModule
        {
            CommandModules.Add(typeof(T));
        }
    }
}