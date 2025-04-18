#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Nefarius.DSharpPlus.Extensions.Hosting;

/// <summary>
///     Optional parameters to pass to <see cref="DiscordClient.ConnectAsync" />.
/// </summary>
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public sealed class DiscordClientConnectOptions
{
    internal DiscordClientConnectOptions()
    {
    }

    /// <summary>
    ///     Optional <see cref="DiscordActivity" /> to pass to <see cref="DiscordClient.ConnectAsync" />.
    /// </summary>
    public DiscordActivity? Activity { internal get; set; } = null;

    /// <summary>
    ///     Optional <see cref="UserStatus" /> to pass to <see cref="DiscordClient.ConnectAsync" />.
    /// </summary>
    public UserStatus? Status { internal get; set; } = null;

    /// <summary>
    ///     Optional <see cref="DateTimeOffset" /> to pass to <see cref="DiscordClient.ConnectAsync" />.
    /// </summary>
    public DateTimeOffset? IdleSince { internal get; set; } = null;
}