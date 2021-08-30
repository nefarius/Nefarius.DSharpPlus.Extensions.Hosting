using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    public static partial class DiscordServiceCollectionExtensions
    {
        [UsedImplicitly]
        public static IServiceCollection AddDiscordWebSocketEventSubscriber<T>(this IServiceCollection services)
            where T : IDiscordWebSocketEventSubscriber
        {
            return services.AddDiscordWebSocketEventSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordWebSocketEventSubscriber(this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordWebSocketEventSubscriber), t);
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordChannelEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordChannelEventsSubscriber
        {
            return services.AddDiscordChannelEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordChannelEventsSubscriber(this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordChannelEventsSubscriber), t);
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordGuildEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordGuildEventsSubscriber
        {
            return services.AddDiscordGuildEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordGuildEventsSubscriber(this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordGuildEventsSubscriber), t);
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordGuildBanEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordGuildBanEventsSubscriber
        {
            return services.AddDiscordGuildBanEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordGuildBanEventsSubscriber(this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordGuildBanEventsSubscriber), t);
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordGuildMemberEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordGuildMemberEventsSubscriber
        {
            return services.AddDiscordGuildMemberEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordGuildMemberEventsSubscriber(this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordGuildMemberEventsSubscriber), t);
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordGuildRoleEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordGuildRoleEventsSubscriber
        {
            return services.AddDiscordGuildRoleEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordGuildRoleEventsSubscriber(this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordGuildRoleEventsSubscriber), t);
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordInviteEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordInviteEventsSubscriber
        {
            return services.AddDiscordInviteEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordInviteEventsSubscriber(this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordInviteEventsSubscriber), t);
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordMessageEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordMessageEventsSubscriber
        {
            return services.AddDiscordMessageEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordMessageEventsSubscriber(this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordMessageEventsSubscriber), t);
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordMessageReactionAddedEventsSubscriber<T>(
            this IServiceCollection services)
            where T : IDiscordMessageReactionEventsSubscriber
        {
            return services.AddDiscordMessageReactionAddedEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordMessageReactionAddedEventsSubscriber(
            this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordMessageReactionEventsSubscriber), t);
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordPresenceUserEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordPresenceUserEventsSubscriber
        {
            return services.AddDiscordPresenceUserEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordPresenceUserEventsSubscriber(this IServiceCollection services,
            Type t)
        {
            return services.AddScoped(typeof(IDiscordPresenceUserEventsSubscriber), t);
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordVoiceEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordVoiceEventsSubscriber
        {
            return services.AddDiscordVoiceEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordVoiceEventsSubscriber(this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordVoiceEventsSubscriber), t);
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordMiscEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordMiscEventsSubscriber
        {
            return services.AddDiscordMiscEventsSubscriber(typeof(T));
        }

        public static IServiceCollection AddDiscordMiscEventsSubscriber(this IServiceCollection services, Type t)
        {
            return services.AddScoped(typeof(IDiscordMiscEventsSubscriber), t);
        }
    }
}