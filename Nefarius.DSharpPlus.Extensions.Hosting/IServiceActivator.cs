using System;

namespace Nefarius.DSharpPlus.Extensions.Hosting;

/// <summary>
///     Initializes optional modules for <see cref="DiscordService" />.
/// </summary>
public interface IServiceActivator
{
    /// <summary>
    ///     Activates the implementation-specific module.
    /// </summary>
    void Activate(IServiceProvider provider);
}