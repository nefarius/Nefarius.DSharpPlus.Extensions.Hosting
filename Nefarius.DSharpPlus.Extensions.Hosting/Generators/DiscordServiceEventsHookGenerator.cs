using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Nefarius.DSharpPlus.Extensions.Hosting.Generators.Util;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Generators;

/// <summary>
///     Builds an internal helper method that wires up all supported event handlers to subscriber proxying.
/// </summary>
[Generator]
public class DiscordServiceEventsHookGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        ClassDeclarationSyntax discordClientClassSyntax = DSharpPlusClientParser.Instance.DiscordClient;

        IEnumerable<EventDeclarationSyntax> eventsSyntax =
            discordClientClassSyntax.Members.OfType<EventDeclarationSyntax>();

        StringBuilder sourceBuilder = new StringBuilder(@"using System;
using System.Linq;
using System.Reflection;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nefarius.DSharpPlus.Extensions.Hosting.Util;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

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

        foreach (EventDeclarationSyntax eventSyntax in eventsSyntax)
        {
            string name = eventSyntax.Identifier.ToString();
            GenericNameSyntax typeSyntax = (GenericNameSyntax)eventSyntax.Type;
            SeparatedSyntaxList<TypeSyntax> arguments = typeSyntax.TypeArgumentList.Arguments;

            string senderType = ((IdentifierNameSyntax)arguments[0]).Identifier.Text;
            string argsType = ((IdentifierNameSyntax)arguments[1]).Identifier.Text;

            sourceBuilder.Append($@"
            Client.{name} += async delegate ({senderType} sender, {argsType} args)
            {{
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

        context.AddSource("DiscordServiceEventsHook.g.cs",
            SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }
}