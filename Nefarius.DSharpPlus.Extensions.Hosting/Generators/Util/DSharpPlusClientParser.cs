﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Generators.Util
{
    /// <summary>
    ///     Converts the latest Discord Client sources into parsed objects.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal class DSharpPlusClientParser
    {
        /// <summary>
        ///     The client source file to download and parse.
        /// </summary>
        public const string DSharpPlusClientSourceUri =
            "https://raw.githubusercontent.com/DSharpPlus/DSharpPlus/master/DSharpPlus/Clients/DiscordClient.Events.cs";

        public const string DSharpPlusIntentsSourceUri =
            "https://raw.githubusercontent.com/DSharpPlus/DSharpPlus/master/DSharpPlus/DiscordIntents.cs";

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

            var response = client.DownloadString(DSharpPlusClientSourceUri);

            var syntaxTree = CSharpSyntaxTree.ParseText(response);

            var root = syntaxTree.GetCompilationUnitRoot();

            //
            // One namespace expected
            // 
            var namespaceSyntax = root.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();

            //
            // One Class definition expected
            // 
            DiscordClient = namespaceSyntax.Members.OfType<ClassDeclarationSyntax>().First();

            response = client.DownloadString(DSharpPlusIntentsSourceUri);

            syntaxTree = CSharpSyntaxTree.ParseText(response);

            root = syntaxTree.GetCompilationUnitRoot();

            //
            // One namespace expected
            // 
            namespaceSyntax = root.Members.OfType<FileScopedNamespaceDeclarationSyntax>().First();

            //
            // One Enum definition expected
            // 
            DiscordIntents = namespaceSyntax.Members.OfType<EnumDeclarationSyntax>().First();
        }

        /// <summary>
        ///     The Discord Client declaration.
        /// </summary>
        public ClassDeclarationSyntax DiscordClient { get; }

        /// <summary>
        ///     The Discord Intents declaration.
        /// </summary>
        public EnumDeclarationSyntax DiscordIntents { get; }

        /// <summary>
        ///     Singleton instance.
        /// </summary>
        public static DSharpPlusClientParser Instance => LazyParser.Value;
    }
}