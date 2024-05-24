using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Nefarius.DSharpPlus.Extensions.Hosting.Generators.Util;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Generators;

/// <summary>
///     Scans for all implemented subscriber implementations and builds a list of required intents for the gateway
///     permissions.
/// </summary>
[Generator]
public class DiscordServiceIntentsBuilderGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        EnumDeclarationSyntax discordIntentsSyntax = DSharpPlusClientParser.Instance.DiscordIntents;

        List<EnumMemberDeclarationSyntax> intents = discordIntentsSyntax.Members.ToList();

        Dictionary<string, HashSet<string>> map = new();

        foreach (EnumMemberDeclarationSyntax intent in intents)
        {
            string intentName = intent.Identifier.Text;

            SyntaxTrivia trivia = intent.GetLeadingTrivia().Single(syntaxTrivia =>
                syntaxTrivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));

            SyntaxNode syntaxNode = trivia.GetStructure();

            if (syntaxNode is null)
            {
                continue;
            }

            SyntaxNode summaryNode = syntaxNode.ChildNodes().Single(node => node.IsKind(SyntaxKind.XmlElement));

            IEnumerable<SyntaxNode> summaryElements =
                summaryNode.ChildNodes().Where(node => node.IsKind(SyntaxKind.XmlElement));

            foreach (SyntaxNode summaryElement in summaryElements)
            {
                IEnumerable<SyntaxNode> crefElements = summaryElement.ChildNodes()
                    .Where(node => node.IsKind(SyntaxKind.XmlEmptyElement));

                foreach (SyntaxNode crefElement in crefElements)
                {
                    SyntaxNode crefAttribute = crefElement.ChildNodes()
                        .Single(node => node.IsKind(SyntaxKind.XmlCrefAttribute));

                    string eventName = crefAttribute.ChildNodes().Single(node => node.IsKind(SyntaxKind.QualifiedCref))
                        .ToString();

                    if (!map.ContainsKey(eventName))
                    {
                        map.Add(eventName, new HashSet<string> { intentName });
                    }
                    else
                    {
                        map[eventName].Add(intentName);
                    }
                }
            }
        }

        StringBuilder sourceBuilder = new(@"using System;
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
    internal partial class DiscordService
    {
        /// <summary>
        ///     Source-generated utility method to auto-detect intents required by registered services.
        /// </summary>
        private static DiscordIntents BuildIntents(IServiceScope scope, DiscordIntents intents)
        {
");

        //
        // Skip "AllUnprivileged" and "All" or code generation breaks
        // 
        foreach (KeyValuePair<string, HashSet<string>> entry in map.Reverse().Skip(2))
        {
            string name = entry.Key.Replace("DiscordClient.", string.Empty);
            HashSet<string> value = entry.Value;

            sourceBuilder.Append($@"
            if (scope.ServiceProvider
                .GetServices(typeof(IDiscord{name}EventSubscriber))
                .Any())
            {{
");

            foreach (string intentName in value)
            {
                sourceBuilder.Append($@"
                intents |= DiscordIntents.{intentName};
");
            }

            sourceBuilder.Append(@"
            }    
");
        }

        sourceBuilder.Append(@"
            return intents;
        }
    }
}
");

        context.AddSource("DiscordServiceIntentsBuilder.g.cs",
            SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }
}