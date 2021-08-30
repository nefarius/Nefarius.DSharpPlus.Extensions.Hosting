using System;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Attributes
{
    /// <summary>
    ///     Marks this class as a receiver of <see cref="IDiscordWebSocketEventSubscriber" /> events.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DiscordWebSocketEventSubscriberAttribute : Attribute
    {
    }
}