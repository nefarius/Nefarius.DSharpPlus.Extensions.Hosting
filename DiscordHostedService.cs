using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    [UsedImplicitly]
    public class DiscordHostedService : IHostedService
    {
        private readonly IDiscordClientService _discordClient;

        private readonly ILogger<DiscordHostedService> _logger;

        private readonly ITracer _tracer;

        public DiscordHostedService(IDiscordClientService discordClient, ITracer tracer,
            ILogger<DiscordHostedService> logger)
        {
            _discordClient = discordClient;
            _tracer = tracer;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (_tracer.BuildSpan(nameof(_discordClient.Client.ConnectAsync)).StartActive(true))
            {
                _logger.LogInformation("Connecting to Discord API...");
                await _discordClient.Client.ConnectAsync();
                _logger.LogInformation("Connected");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _discordClient.Client.DisconnectAsync();
        }
    }
}