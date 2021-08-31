using System;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Events;

namespace Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Attributes
{
    /// <summary>
    ///     Marks this class as a receiver of <see cref="IDiscordCommandsNextEventsSubscriber" /> events.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DiscordCommandsNextEventsSubscriberAttribute : Attribute
    {
    }
}