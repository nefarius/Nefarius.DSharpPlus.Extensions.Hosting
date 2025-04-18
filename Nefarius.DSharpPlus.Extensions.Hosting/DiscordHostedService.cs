using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Nefarius.DSharpPlus.Extensions.Hosting;

/// <summary>
///     Brings a <see cref="IDiscordClientService" /> online.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class DiscordHostedService(
    IDiscordClientService discordClient,
    ILogger<DiscordHostedService> logger,
    IOptions<DiscordClientConnectOptions> connectOptions)
    : IHostedService
{
    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        ((DiscordService)discordClient).Initialize();

        DiscordClientConnectOptions opts = connectOptions.Value;

        logger.LogInformation("Connecting to Discord API...");
        await discordClient.Client.ConnectAsync(opts.Activity, opts.Status, opts.IdleSince);
        logger.LogInformation("Connected");
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await discordClient.Client.DisconnectAsync();
    }
}