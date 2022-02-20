using System.Linq;
using System.Net;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Generators
{
    [Generator]
    public class DiscordClientEventsInterfacesGenerator : ISourceGenerator
    {
        private const string DSharpPlusSourceUri =
            "https://raw.githubusercontent.com/DSharpPlus/DSharpPlus/master/DSharpPlus/Clients/DiscordClient.Events.cs";

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            using var client = new WebClient();

            // Required or result is HTTP-403
            client.Headers["User-Agent"] =
                "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                "(compatible; MSIE 6.0; Windows NT 5.1; " +
                ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

            var response = client.DownloadString(DSharpPlusSourceUri);

            var syntaxTree = CSharpSyntaxTree.ParseText(response);

            var root = syntaxTree.GetCompilationUnitRoot();

            var namespaceSyntax = root.Members.OfType<NamespaceDeclarationSyntax>().First();

            var discordClientClassSyntax = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();

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
    public sealed class Discord{name}SubscriberAttribute : Attribute {{ }}
");

                sourceBuilder.Append($@"
    public interface IDiscord{name}Subscriber
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