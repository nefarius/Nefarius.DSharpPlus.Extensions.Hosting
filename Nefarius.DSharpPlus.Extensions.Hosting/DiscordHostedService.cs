using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    /// <summary>
    ///     Brings a <see cref="IDiscordClientService" /> online.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class DiscordHostedService : IHostedService
    {
        private readonly IDiscordClientService _discordClient;

        private readonly ILogger<DiscordHostedService> _logger;

        private readonly ITracer _tracer;

        public DiscordHostedService(
            IDiscordClientService discordClient,
            ITracer tracer,
            ILogger<DiscordHostedService> logger)
        {
            _discordClient = discordClient;
            _tracer = tracer;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ((DiscordService) _discordClient).Initialize();

            using (_tracer.BuildSpan(nameof(_discordClient.Client.ConnectAsync)).StartActive(true))
            {
                _logger.LogInformation("Connecting to Discord API...");
                await _discordClient.Client.ConnectAsync();
                _logger.LogInformation("Connected");
            }
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _discordClient.Client.DisconnectAsync();
        }
    }
}