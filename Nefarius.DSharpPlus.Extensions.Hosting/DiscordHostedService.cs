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
public class DiscordHostedService : IHostedService
{
    private readonly IOptions<DiscordClientConnectOptions> _connectOptions;
    private readonly IDiscordClientService _discordClient;
    private readonly ILogger<DiscordHostedService> _logger;

    public DiscordHostedService(
        IDiscordClientService discordClient,
        ILogger<DiscordHostedService> logger,
        IOptions<DiscordClientConnectOptions> connectOptions
    )
    {
        _discordClient = discordClient;
        _logger = logger;
        _connectOptions = connectOptions;
    }

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        ((DiscordService)_discordClient).Initialize();

        var opts = _connectOptions.Value;

        _logger.LogInformation("Connecting to Discord API...");
        await _discordClient.Client.ConnectAsync(opts.Activity, opts.Status, opts.IdleSince);
        _logger.LogInformation("Connected");
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _discordClient.Client.DisconnectAsync();
    }
}