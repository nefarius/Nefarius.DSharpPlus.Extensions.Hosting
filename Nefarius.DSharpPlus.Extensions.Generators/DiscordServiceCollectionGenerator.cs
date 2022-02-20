using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Nefarius.DSharpPlus.Extensions.Generators.Util;

namespace Nefarius.DSharpPlus.Extensions.Generators
{
    [Generator]
    public class DiscordServiceCollectionGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var discordClientClassSyntax = DSharpPlusClientParser.Instance.DiscordClient;

            var eventsSyntax = discordClientClassSyntax.Members.OfType<EventDeclarationSyntax>().ToList();

            var sourceBuilder = new StringBuilder(@"using System;
using DSharpPlus;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nefarius.DSharpPlus.Extensions.Hosting.Attributes;
using Nefarius.DSharpPlus.Extensions.Hosting.Util;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;
using OpenTracing;
using OpenTracing.Mock;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    public static partial class DiscordServiceCollectionExtensions
    {");

            foreach (var name in eventsSyntax.Select(eventSyntax => eventSyntax.Identifier.ToString()))
                sourceBuilder.Append($@"
        public static IServiceCollection AddDiscord{name}EventSubscriber<T>(this IServiceCollection services)
            where T : IDiscord{name}EventSubscriber
        {{
            return services.AddDiscord{name}EventSubscriber(typeof(T));
        }}

        public static IServiceCollection AddDiscord{name}EventSubscriber(this IServiceCollection services, Type t)
        {{
            return services.AddScoped(typeof(IDiscord{name}EventSubscriber), t);
        }}
");

            sourceBuilder.Append(@"
        private static void RegisterSubscribers(IServiceCollection services)
        {
");

            foreach (var name in eventsSyntax.Select(eventSyntax => eventSyntax.Identifier.ToString()))
                sourceBuilder.Append($@"
            foreach (var type in AssemblyTypeHelper.GetTypesWith<Discord{name}EventSubscriberAttribute>())
                services.AddDiscord{name}EventSubscriber(type);
");

            sourceBuilder.Append(@"
        }
    }
}
");

            context.AddSource("DiscordServiceCollectionGenerated",
                SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}