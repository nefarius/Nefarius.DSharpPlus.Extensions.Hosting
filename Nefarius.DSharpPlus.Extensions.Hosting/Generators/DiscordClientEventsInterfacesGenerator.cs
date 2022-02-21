using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Nefarius.DSharpPlus.Extensions.Hosting.Generators.Util;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Generators
{
    /// <summary>
    ///     Builds the attributes and interfaces to decorate and implement event subscribers
    /// </summary>
    [Generator]
    public class DiscordClientEventsInterfacesGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var discordClientClassSyntax = DSharpPlusClientParser.Instance.DiscordClient;

            var eventsSyntax = discordClientClassSyntax.Members.OfType<EventDeclarationSyntax>();

            var sourceBuilder = new StringBuilder(@"using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{");

            foreach (var eventSyntax in eventsSyntax)
            {
                var name = eventSyntax.Identifier.ToString();
                var typeSyntax = (GenericNameSyntax)eventSyntax.Type;
                var arguments = typeSyntax.TypeArgumentList.Arguments;

                var senderType = ((IdentifierNameSyntax)arguments[0]).Identifier.Text;
                var argsType = ((IdentifierNameSyntax)arguments[1]).Identifier.Text;

                sourceBuilder.Append($@"
    /// <summary>
    ///     Marks this class as a receiver of <see cref=""IDiscord{name}Subscriber"" /> events.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class Discord{name}EventSubscriberAttribute : Attribute {{ }}
");

                sourceBuilder.Append($@"
    /// <summary>
    ///     Implements a DiscordOn{name} event handler.
    /// </summary>
    public interface IDiscord{name}EventSubscriber
    {{
        public Task DiscordOn{name}({senderType} sender, {argsType} args);
    }}
");
            }

            sourceBuilder.Append(@"
}
");

            context.AddSource("DiscordClientEventsInterfacesGenerated", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}