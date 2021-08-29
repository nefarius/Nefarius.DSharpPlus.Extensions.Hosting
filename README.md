<img src="assets/NSS-128x128.png" align="right" />

# DSharpPlus hosting extensions

An extension for [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus) to make hosting a Discord bot in [.NET Core Worker Services](https://docs.microsoft.com/en-us/dotnet/core/extensions/workers) easier.

[![Build status](https://ci.appveyor.com/api/projects/status/qgix03imre2tya71?svg=true)](https://ci.appveyor.com/project/nefarius/nefarius-dsharpplus-extensions-hosting)

## About

Work in progress!

## To-Do

- [ ] Documentation!
- [ ] Implement missing events
  - [ ] Integration
  - [ ] Stage Instance
  - [ ] Misc (partially)

## Package overview

### Nefarius.DSharpPlus.Extensions.Hosting

[![NuGet](https://img.shields.io/nuget/vpre/Nefarius.DSharpPlus.Extensions.Hosting.svg)](https://nuget.org/packages/Nefarius.DSharpPlus.Extensions.Hosting)

The core library for [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus), required to set up a Discord client as a hosted service.

### Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting

[![NuGet](https://img.shields.io/nuget/vpre/Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.svg)](https://nuget.org/packages/Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting)

Optional. Adds support for [DSharpPlus.CommandsNext](https://dsharpplus.github.io/articles/commands/intro.html) extension.

### Nefarius.DSharpPlus.Interactivity.Extensions.Hosting

[![NuGet](https://img.shields.io/nuget/vpre/Nefarius.DSharpPlus.Interactivity.Extensions.Hosting.svg)](https://nuget.org/packages/Nefarius.DSharpPlus.Interactivity.Extensions.Hosting)

Optional. Adds support for [DSharpPlus.Interactivity](https://dsharpplus.github.io/articles/interactivity.html) extension.

### Nefarius.DSharpPlus.VoiceNext.Extensions.Hosting

[![NuGet](https://img.shields.io/nuget/vpre/Nefarius.DSharpPlus.VoiceNext.Extensions.Hosting.svg)](https://nuget.org/packages/Nefarius.DSharpPlus.VoiceNext.Extensions.Hosting)

Optional. Adds support for [DSharpPlus.VoiceNext](https://dsharpplus.github.io/articles/audio/voicenext/prerequisites.html)

### Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting

[![NuGet](https://img.shields.io/nuget/vpre/Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.svg)](https://nuget.org/packages/Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting)

Optional. Adds support for [DSharpPlus.SlashCommands](https://github.com/IDoEverything/DSharpPlus.SlashCommands) extension (3rd party).
