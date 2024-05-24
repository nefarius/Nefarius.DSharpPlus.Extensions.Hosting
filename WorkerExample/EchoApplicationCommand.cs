using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace WorkerExample;

[SlashCommandGroup("demo", "Demo commands.")]
internal sealed class EchoApplicationCommand : ApplicationCommandModule
{
    [SlashRequirePermissions(DiscordPermissions.SendMessages)]
    [SlashCommand("Ping", "Sends a ping.")]
    public async Task Echo(
        InteractionContext ctx
    )
    {
        await ctx.DeferAsync();

        var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(new DiscordEmbedBuilder
        {
            Title = "Ping response",
            Description = $"{emoji} Pong! Ping: {ctx.Client.Ping}ms"
        }));
    }
}