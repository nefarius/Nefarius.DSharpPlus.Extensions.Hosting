using System;
using System.Diagnostics.CodeAnalysis;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace Nefarius.DSharpPlus.Interactivity.Extensions.Hosting
{
	/// <summary>
	///     Extensions methods for <see cref="IServiceCollection"/>.
	/// </summary>
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public static class DiscordServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds Interactivity extension to <see cref="IDiscordClientService"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="InteractivityConfiguration"/>.</param>
        /// <param name="extension">The <see cref="InteractivityExtension"/></param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddDiscordInteractivity(
            this IServiceCollection services,
            Action<InteractivityConfiguration> configuration,
            Action<InteractivityExtension?> extension = null
        )
        {
            services.AddSingleton(typeof(IDiscordExtensionConfiguration), provider =>
            {
                var options = new InteractivityConfiguration();

                configuration(options);

                var discord = provider.GetRequiredService<IDiscordClientService>().Client;

                var ext = discord.UseInteractivity(options);

                extension?.Invoke(ext);

                //
                // This is intentional; we don't need this "service", just the execution flow ;)
                // 
                return null;
            });

            return services;
        }
    }
}