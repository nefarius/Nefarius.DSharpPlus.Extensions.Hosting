using System;
using System.Diagnostics.CodeAnalysis;

using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Events;

namespace Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Attributes;

/// <summary>
///     Marks this class as a receiver of <see cref="IDiscordCommandsNextEventsSubscriber" /> events.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class DiscordCommandsNextEventsSubscriberAttribute : Attribute
{
}