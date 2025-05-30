using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Nefarius.DSharpPlus.Extensions.Hosting.Generators.Util;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Generators;

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
        ClassDeclarationSyntax discordClientClassSyntax = DSharpPlusClientParser.Instance.DiscordClient;

        IEnumerable<EventDeclarationSyntax> eventsSyntax =
            discordClientClassSyntax.Members.OfType<EventDeclarationSyntax>();

        StringBuilder sourceBuilder = new(@"using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using JetBrains.Annotations;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events
{");

        foreach (EventDeclarationSyntax eventSyntax in eventsSyntax)
        {
            string name = eventSyntax.Identifier.ToString();
            GenericNameSyntax typeSyntax = (GenericNameSyntax)eventSyntax.Type;
            SeparatedSyntaxList<TypeSyntax> arguments = typeSyntax.TypeArgumentList.Arguments;

            string senderType = ((IdentifierNameSyntax)arguments[0]).Identifier.Text;
            string argsType = ((IdentifierNameSyntax)arguments[1]).Identifier.Text;

            sourceBuilder.Append($@"
    /// <summary>
    ///     Marks this class as a receiver of <see cref=""IDiscord{name}EventSubscriber"" /> events.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [MeansImplicitUse]
    public sealed class Discord{name}EventSubscriberAttribute : Attribute {{ }}
");

            sourceBuilder.Append($@"
    /// <summary>
    ///     Implements a DiscordOn{name} event handler.
    /// </summary>
    public interface IDiscord{name}EventSubscriber
    {{
        /// <summary>
        ///     Handles <see cref=""{argsType}"" />.
        /// </summary>
        public Task DiscordOn{name}({senderType} sender, {argsType} args);
    }}
");
        }

        sourceBuilder.Append(@"
}
");

        context.AddSource("DiscordClientEventsInterfaces.g.cs",
            SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }
}