using System;
using System.Linq;
using System.Net;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Nefarius.DSharpPlus.Extensions.Generators.Util
{
    /// <summary>
    ///     Converts the latest Discord Client sources into parsed objects.
    /// </summary>
    internal class DSharpPlusClientParser
    {
        /// <summary>
        ///     The source file to download and parse.
        /// </summary>
        public const string DSharpPlusSourceUri =
            "https://raw.githubusercontent.com/DSharpPlus/DSharpPlus/master/DSharpPlus/Clients/DiscordClient.Events.cs";

        private static readonly Lazy<DSharpPlusClientParser> LazyParser = new(() => new DSharpPlusClientParser());

        private DSharpPlusClientParser()
        {
            using var client = new WebClient();

            // 
            // Required or result is HTTP-403
            // 
            client.Headers["User-Agent"] =
                "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                "(compatible; MSIE 6.0; Windows NT 5.1; " +
                ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

            var response = client.DownloadString(DSharpPlusSourceUri);

            var syntaxTree = CSharpSyntaxTree.ParseText(response);

            var root = syntaxTree.GetCompilationUnitRoot();

            //
            // One namespace expected
            // 
            var namespaceSyntax = root.Members.OfType<NamespaceDeclarationSyntax>().First();

            //
            // One class definition expected
            // 
            DiscordClient = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();
        }

        /// <summary>
        ///     The Discord Client declaration.
        /// </summary>
        public ClassDeclarationSyntax DiscordClient { get; }

        /// <summary>
        ///     Singleton instance.
        /// </summary>
        public static DSharpPlusClientParser Instance => LazyParser.Value;
    }
}