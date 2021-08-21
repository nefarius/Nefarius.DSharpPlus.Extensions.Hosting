using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    [UsedImplicitly]
    public static class DiscordServiceCollectionExtensions
    {
        [UsedImplicitly]
        public static IServiceCollection AddDiscord(
            this IServiceCollection services,
            Action<DiscordServiceOptions> configure
        )
        {
            services.AddOptions();

            services.Configure(configure);

            services.TryAddSingleton<IDiscordClientService, DiscordService>();

            return services;
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordHostedService(
            this IServiceCollection services
        )
        {
            return services.AddHostedService<DiscordHostedService>();
        }

        #region Subscribers
        
        [UsedImplicitly]
        public static IServiceCollection AddDiscordChannelEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordChannelEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordChannelEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordGuildEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordGuildEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordGuildEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordGuildBanEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordGuildBanEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordGuildBanEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordGuildMemberEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordGuildMemberEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordGuildMemberEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordGuildRoleEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordGuildRoleEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordGuildRoleEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordInviteEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordInviteEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordInviteEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordMessageEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordMessageEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordMessageEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordMessageReactionAddedEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordMessageReactionAddedEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordMessageReactionAddedEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordPresenceUserEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordPresenceUserEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordPresenceUserEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordVoiceEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordVoiceEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordVoiceEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordMiscEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordMiscEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordMiscEventsSubscriber), typeof(T));
        }

        #endregion
    }
}