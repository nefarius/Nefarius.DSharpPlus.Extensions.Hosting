<img src="assets/NSS-128x128.png" align="right" />

# DSharpPlus hosting extensions

An extension for [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus) to make hosting a Discord bot in [.NET Core Worker Services](https://docs.microsoft.com/en-us/dotnet/core/extensions/workers) easier.

[![Build status](https://ci.appveyor.com/api/projects/status/qgix03imre2tya71?svg=true)](https://ci.appveyor.com/project/nefarius/nefarius-dsharpplus-extensions-hosting)

## About

**This is not an official DSharpPlus extension, use it at your own risk!**

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

## Documentation

If you're already familiar with .NET Core Workers or ASP.NET Core you'll have your bot up and running in seconds 👌

You can also take a look at [the reference example](../../WorkerExample) of this repository.

### Setup

Create a new .NET Core Worker project either via Visual Studio templates or using the command `dotnet new worker` in a fresh directory.

The current version of the library depends on the DSharpPlus nightly version. If you're using the stable nuget version, [update to the nightly version](https://dsharpplus.github.io/articles/misc/nightly_builds.html).

Add the core hosting package (and optional extensions, if you need them) via NuGet package manager.

### Implementation

Most of the heavy lifting is done in the `ConfigureServices` method, so we will focus on that. To get a bare basic Discord bot running, all you need to do is register the client service and the hosted background service:

```csharp
//
// Tracer is required, if you don't use one, use the MockTracer
// 
services.AddSingleton<ITracer>(provider => new MockTracer());

//
// Adds DiscordClient singleton service you can use everywhere
// 
services.AddDiscord(options =>
{
 //
 // Minimum required configuration
 // 
 options.Token = "recommended to read bot token from configuration file";
});

//
// Automatically host service and connect to gateway on boot
// 
services.AddDiscordHostedService();
```

That's pretty much it! When you launch your worker with a valid bot token you should see your bot come online in an instant, congratulations! ✨

You probably wonder what's the deal with the tracing dependency. I've taken liberty to implement [OpenTracing](https://github.com/opentracing/opentracing-csharp) within all event subscribers, so if your bot stuggles with performance, you can easily analyse it with the addition of e.g. [Jaeger Tracing](https://github.com/jaegertracing/jaeger-client-csharp). If you don't know what that means or don't care about tracing at all, just register the mock tracer as displayed above and it will be happy.

### Handling Discord Events

Now to the actual connveninece feature of this library! Creating one (or more) class(es) that handle events, like when a guild came online or a message got created. Let's wire one up that gets general guild and member change events:

```csharp
[DiscordGuildEventsSubscriber]
[DiscordGuildMemberEventsSubscriber]
internal class BotModuleForGuildAndMemberEvents : 
 IDiscordGuildEventsSubscriber,
 IDiscordGuildMemberEventsSubscriber
{
 public Task DiscordOnGuildCreated(DiscordClient sender, GuildCreateEventArgs args)
 {
  return Task.CompletedTask;
 }

 public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args)
 {
  //
  // To see some action, output the guild name
  // 
  Console.WriteLine(args.Guild.Name);

  return Task.CompletedTask;
 }

 public Task DiscordOnGuildUpdated(DiscordClient sender, GuildUpdateEventArgs args)
 {
  return Task.CompletedTask;
 }

 public Task DiscordOnGuildDeleted(DiscordClient sender, GuildDeleteEventArgs args)
 {
  return Task.CompletedTask;
 }

 public Task DiscordOnGuildUnavailable(DiscordClient sender, GuildDeleteEventArgs args)
 {
  return Task.CompletedTask;
 }

 public Task DiscordOnGuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs args)
 {
  return Task.CompletedTask;
 }

 public Task DiscordOnGuildEmojisUpdated(DiscordClient sender, GuildEmojisUpdateEventArgs args)
 {
  return Task.CompletedTask;
 }

 public Task DiscordOnGuildStickersUpdated(DiscordClient sender, GuildStickersUpdateEventArgs args)
 {
  return Task.CompletedTask;
 }

 public Task DiscordOnGuildIntegrationsUpdated(DiscordClient sender, GuildIntegrationsUpdateEventArgs args)
 {
  return Task.CompletedTask;
 }

 public Task DiscordOnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs args)
 {
  return Task.CompletedTask;
 }

 public Task DiscordOnGuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs args)
 {
  return Task.CompletedTask;
 }

 public Task DiscordOnGuildMemberUpdated(DiscordClient sender, GuildMemberUpdateEventArgs args)
 {
  return Task.CompletedTask;
 }

 public Task DiscordOnGuildMembersChunked(DiscordClient sender, GuildMembersChunkEventArgs args)
 {
  return Task.CompletedTask;
 }
}
```

Now let's dissect what is happening here. The class gets decorated by the attributes `DiscordGuildEventsSubscriber` and `DiscordGuildMemberEventsSubscriber` (hint: you can use only one attribute for the event group you're interested in, you can use many more on the same class, doesn't matter, your choice) which causes it to get **automatically registered as subscribers for these event groups**.

An alternative approach to registration is manually calling the extension methods, like

```csharp
services.AddDiscordGuildEventsSubscriber<BotModuleForGuildAndMemberEvents>();
services.AddDiscordGuildMemberEventsSubscriber<BotModuleForGuildAndMemberEvents>();
```

from within `ConfigureServices`. Using the attributes instead ensures you don't forget to register your subscribers while coding vigorously!

Implementing the interfaces `IDiscordGuildEventsSubscriber` and `IDiscordGuildMemberEventsSubscriber` ensures your subscriber class is actually callable by the Discord Client Service. You must complete every event callback you're not interested in with `return Task.CompletedTask;` as demonstrated or it will result in errors. In the example above we are only interested in `DiscordOnGuildAvailable` and print the guild name to the console. I'm sure you can think of more exciting tasks!

And last but not least; your subscriber classes are fully dependency injection aware! You can access services via classic constructor injection:

```csharp
private readonly ILogger<BotModuleForGuildAndMemberEvents> _logger;

private readonly ITracer _tracer;

public BotModuleForGuildAndMemberEvents(
 ILogger<BotModuleForGuildAndMemberEvents> logger,
 ITracer tracer
)
{
 _logger = logger;
 _tracer = tracer;
}
```

You can even inject **scoped services**, the subscriber object get invoked in their own scope by default. This allows for easy access for e.g. database contexts within each subscriber. Neat!
