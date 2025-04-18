using System;
using System.Diagnostics.CodeAnalysis;

using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.Events;

namespace Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.Attributes;

/// <summary>
///     Marks this class as a receiver of <see cref="IDiscordSlashCommandsEventsSubscriber" /> events.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class DiscordSlashCommandsEventsSubscriberAttribute : Attribute
{
}