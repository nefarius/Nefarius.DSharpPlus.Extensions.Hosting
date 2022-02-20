using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Util
{
    internal static class ServiceScopeExtensions
    {
        public static IList<IDiscordChannelEventsSubscriber> GetDiscordChannelEventsSubscribers(
            this IServiceScope scope
        )
        {
            return scope.ServiceProvider
                .GetServices(typeof(IDiscordChannelEventsSubscriber))
                .Cast<IDiscordChannelEventsSubscriber>()
                .ToList();
        }

        public static IList<IDiscordGuildEventsSubscriber> GetDiscordGuildEventsSubscribers(
            this IServiceScope scope
        )
        {
            return scope.ServiceProvider
                .GetServices(typeof(IDiscordGuildEventsSubscriber))
                .Cast<IDiscordGuildEventsSubscriber>()
                .ToList();
        }

        public static IList<IDiscordGuildBanEventsSubscriber> GetDiscordGuildBanEventsSubscribers(
            this IServiceScope scope
        )
        {
            return scope.ServiceProvider
                .GetServices(typeof(IDiscordGuildBanEventsSubscriber))
                .Cast<IDiscordGuildBanEventsSubscriber>()
                .ToList();
        }

        public static IList<IDiscordGuildMemberEventsSubscriber> GetDiscordGuildMemberEventsSubscribers(
            this IServiceScope scope
        )
        {
            return scope.ServiceProvider
                .GetServices(typeof(IDiscordGuildMemberEventsSubscriber))
                .Cast<IDiscordGuildMemberEventsSubscriber>()
                .ToList();
        }

        public static IList<IDiscordGuildRoleEventsSubscriber> GetDiscordGuildRoleEventsSubscribers(
            this IServiceScope scope
        )
        {
            return scope.ServiceProvider
                .GetServices(typeof(IDiscordGuildRoleEventsSubscriber))
                .Cast<IDiscordGuildRoleEventsSubscriber>()
                .ToList();
        }

        public static IList<IDiscordInviteEventsSubscriber> GetDiscordInviteEventsSubscribers(
            this IServiceScope scope
        )
        {
            return scope.ServiceProvider
                .GetServices(typeof(IDiscordInviteEventsSubscriber))
                .Cast<IDiscordInviteEventsSubscriber>()
                .ToList();
        }

        public static IList<IDiscordMessageEventsSubscriber> GetDiscordMessageEventsSubscribers(
            this IServiceScope scope
        )
        {
            return scope.ServiceProvider
                .GetServices(typeof(IDiscordMessageEventsSubscriber))
                .Cast<IDiscordMessageEventsSubscriber>()
                .ToList();
        }

        public static IList<IDiscordMessageReactionEventsSubscriber> GetDiscordMessageReactionAddedEventsSubscribers(
            this IServiceScope scope
        )
        {
            return scope.ServiceProvider
                .GetServices(typeof(IDiscordMessageReactionEventsSubscriber))
                .Cast<IDiscordMessageReactionEventsSubscriber>()
                .ToList();
        }

        public static IList<IDiscordPresenceUserEventsSubscriber> GetDiscordPresenceUserEventsSubscribers(
            this IServiceScope scope
        )
        {
            return scope.ServiceProvider
                .GetServices(typeof(IDiscordPresenceUserEventsSubscriber))
                .Cast<IDiscordPresenceUserEventsSubscriber>()
                .ToList();
        }

        public static IList<IDiscordVoiceEventsSubscriber> GetDiscordVoiceEventsSubscribers(
            this IServiceScope scope
        )
        {
            return scope.ServiceProvider
                .GetServices(typeof(IDiscordVoiceEventsSubscriber))
                .Cast<IDiscordVoiceEventsSubscriber>()
                .ToList();
        }
    }
}
