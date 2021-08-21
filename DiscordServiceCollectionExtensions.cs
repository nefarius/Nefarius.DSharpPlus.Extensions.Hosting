using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

        [UsedImplicitly]
        public static IServiceCollection AddDiscordGuildEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordGuildEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordGuildEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordGuildMemberEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordGuildMemberEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordGuildMemberEventsSubscriber), typeof(T));
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
        public static IServiceCollection AddDiscordVoiceEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordVoiceEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordVoiceEventsSubscriber), typeof(T));
        }

        [UsedImplicitly]
        public static IServiceCollection AddDiscordComponentInteractionEventsSubscriber<T>(this IServiceCollection services)
            where T : IDiscordComponentInteractionEventsSubscriber
        {
            return services.AddScoped(typeof(IDiscordComponentInteractionEventsSubscriber), typeof(T));
        }
    }
}