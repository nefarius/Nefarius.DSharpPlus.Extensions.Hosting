using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Nefarius.DSharpPlus.Extensions.Hosting.Generators.Util;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Generators
{
    [Generator]
    public class DiscordServiceIntentsBuilderGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var discordIntentsSyntax = DSharpPlusClientParser.Instance.DiscordIntents;

            var intents = discordIntentsSyntax.Members.ToList();

            var map = new Dictionary<string, HashSet<string>>();

            foreach (var intent in intents)
            {
                var intentName = intent.Identifier.Text;

                var trivia = intent.GetLeadingTrivia().Single(syntaxTrivia =>
                    syntaxTrivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));

                var syntaxNode = trivia.GetStructure();

                if (syntaxNode is null)
                    continue;

                var summaryNode = syntaxNode.ChildNodes().Single(node => node.IsKind(SyntaxKind.XmlElement));

                var summaryElements = summaryNode.ChildNodes().Where(node => node.IsKind(SyntaxKind.XmlElement));

                foreach (var summaryElement in summaryElements)
                {
                    var crefElements = summaryElement.ChildNodes()
                        .Where(node => node.IsKind(SyntaxKind.XmlEmptyElement));

                    foreach (var crefElement in crefElements)
                    {
                        var crefAttribute = crefElement.ChildNodes()
                            .Single(node => node.IsKind(SyntaxKind.XmlCrefAttribute));

                        var eventName = crefAttribute.ChildNodes().Single(node => node.IsKind(SyntaxKind.QualifiedCref))
                            .ToString();

                        if (!map.ContainsKey(eventName))
                            map.Add(eventName, new HashSet<string> { intentName });
                        else
                            map[eventName].Add(intentName);
                    }
                }
            }

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
        ///     Source-generated utility method to auto-detect intents required by registered services.
        /// </summary>
        private static DiscordIntents BuildIntents(IServiceScope scope, DiscordIntents intents)
        {
");

            //
            // Skip "AllUnprivileged" and "All" or code generation breaks
            // 
            foreach (var entry in map.Reverse().Skip(2))
            {
                var name = entry.Key.Replace("DiscordClient.", string.Empty);
                var value = entry.Value;

                sourceBuilder.Append($@"
            if (scope.ServiceProvider
                .GetServices(typeof(IDiscord{name}EventSubscriber))
                .Any())
            {{
");

                foreach (var intentName in value)
                    sourceBuilder.Append($@"
                intents |= DiscordIntents.{intentName};
");

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

            context.AddSource("DiscordServiceIntentsBuilderGenerated", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}