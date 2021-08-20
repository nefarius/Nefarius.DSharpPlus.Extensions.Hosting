using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    public interface IDiscordGuildEventsSubscriber
    {
        public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args);

        public Task DiscordOnGuildUpdated(DiscordClient sender, GuildUpdateEventArgs args);
    }

    public interface IDiscordGuildMemberEventsSubscriber
    {
        public Task DiscordOnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs args);

        public Task DiscordOnGuildMemberUpdated(DiscordClient sender, GuildMemberUpdateEventArgs args);

        public Task DiscordOnGuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs args);
    }

    public interface IDiscordInviteEventsSubscriber
    {
        public Task DiscordOnInviteCreated(DiscordClient sender, InviteCreateEventArgs args);
    }

    public interface IDiscordMessageEventsSubscriber
    {
        public Task DiscordOnMessageCreated(DiscordClient sender, MessageCreateEventArgs args);
    }

    public interface IDiscordVoiceEventsSubscriber
    {
        public Task DiscordOnVoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs args);
    }

    public interface IDiscordComponentInteractionEventsSubscriber
    {
        public Task DiscordOnComponentInteractionCreated(DiscordClient sender,
            ComponentInteractionCreateEventArgs args);
    }
}