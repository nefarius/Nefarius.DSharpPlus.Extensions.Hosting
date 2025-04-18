using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Nefarius.DSharpPlus.Extensions.Hosting.Generators.Util;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Generators;

[Generator]
public class DiscordServiceCollectionGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        ClassDeclarationSyntax discordClientClassSyntax = DSharpPlusClientParser.Instance.DiscordClient;

        List<EventDeclarationSyntax> eventsSyntax =
            discordClientClassSyntax.Members.OfType<EventDeclarationSyntax>().ToList();

        StringBuilder sourceBuilder = new(@"using System;
using DSharpPlus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nefarius.DSharpPlus.Extensions.Hosting.Util;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    public static partial class DiscordServiceCollectionExtensions
    {");

        foreach (string name in eventsSyntax.Select(eventSyntax => eventSyntax.Identifier.ToString()))
        {
            sourceBuilder.Append($@"
        /// <summary>
        ///     Registers an event subscriber implementation.
        /// </summary>
        /// <typeparam name=""T"">An implementation of <see cref=""IDiscord{name}EventSubscriber""/>.</typeparam>
        /// <param name=""services"">The <see cref=""IServiceCollection""/>.</param>
        /// <returns>The <see cref=""IServiceCollection""/>.</returns>
        public static IServiceCollection AddDiscord{name}EventSubscriber<T>(this IServiceCollection services)
            where T : IDiscord{name}EventSubscriber
        {{
            return services.AddDiscord{name}EventSubscriber(typeof(T));
        }}

        /// <summary>
        ///     Registers an event subscriber implementation.
        /// </summary>
        /// <param name=""services"">The <see cref=""IServiceCollection""/>.</param>
        /// <param name=""t"">The type of the subscriber implementation.</param>
        /// <returns>The <see cref=""IServiceCollection""/>.</returns>
        public static IServiceCollection AddDiscord{name}EventSubscriber(this IServiceCollection services, Type t)
        {{
            return services.AddScoped(typeof(IDiscord{name}EventSubscriber), t);
        }}
");
        }

        sourceBuilder.Append(@"
        private static void RegisterSubscribers(IServiceCollection services)
        {
");

        foreach (string name in eventsSyntax.Select(eventSyntax => eventSyntax.Identifier.ToString()))
        {
            sourceBuilder.Append($@"
            foreach (var type in AssemblyTypeHelper.GetTypesWith<Discord{name}EventSubscriberAttribute>())
                services.AddDiscord{name}EventSubscriber(type);
");
        }

        sourceBuilder.Append(@"
        }
    }
}
");

        context.AddSource("DiscordServiceCollectionGenerated",
            SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }
}