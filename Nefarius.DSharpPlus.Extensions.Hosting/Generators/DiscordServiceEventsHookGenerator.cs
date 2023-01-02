using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Nefarius.DSharpPlus.Extensions.Hosting.Generators.Util;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Generators
{
    [Generator]
    public class DiscordServiceEventsHookGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var discordClientClassSyntax = DSharpPlusClientParser.Instance.DiscordClient;

            var eventsSyntax = discordClientClassSyntax.Members.OfType<EventDeclarationSyntax>();

            var sourceBuilder = new StringBuilder(@"using System;
using System.Linq;
using System.Reflection;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nefarius.DSharpPlus.Extensions.Hosting.Util;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;
using OpenTracing;

namespace Nefarius.DSharpPlus.Extensions.Hosting
{
    public partial class DiscordService
    {
        /// <summary>
        ///     Source-generated utility method to hook event handlers to services.
        /// </summary>
        internal void HookEvents()
        {
");

            foreach (var eventSyntax in eventsSyntax)
            {
                var name = eventSyntax.Identifier.ToString();
                var typeSyntax = (GenericNameSyntax)eventSyntax.Type;
                var arguments = typeSyntax.TypeArgumentList.Arguments;

                var senderType = ((IdentifierNameSyntax)arguments[0]).Identifier.Text;
                var argsType = ((IdentifierNameSyntax)arguments[1]).Identifier.Text;

                sourceBuilder.Append($@"
            Client.{name} += async delegate ({senderType} sender, {argsType} args)
            {{
                using var workScope = _tracer
                    .BuildSpan(nameof(Client.{name}))
                    .IgnoreActiveSpan()
                    .StartActive(true);

                using var scope = _serviceProvider.CreateScope();
                
                var subscribers = scope.ServiceProvider
                    .GetServices(typeof(IDiscord{name}EventSubscriber))
                    .Cast<IDiscord{name}EventSubscriber>();

                foreach (var eventSubscriber in subscribers)
                    await eventSubscriber.DiscordOn{name}(sender, args);
            }};
");
            }

            sourceBuilder.Append(@"
        }
    }
}
");

            context.AddSource("DiscordServiceEventsHookGenerated",
                SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}