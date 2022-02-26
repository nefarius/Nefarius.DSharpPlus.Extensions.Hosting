<img src="assets/NSS-128x128.png" align="right" />

# DSharpPlus hosting extensions

An extension for [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus) to make hosting a Discord bot in [.NET Core Worker Services](https://docs.microsoft.com/en-us/dotnet/core/extensions/workers) easier.

[![Build status](https://ci.appveyor.com/api/projects/status/qgix03imre2tya71?svg=true)](https://ci.appveyor.com/project/nefarius/nefarius-dsharpplus-extensions-hosting) ![GitHub](https://img.shields.io/github/license/nefarius/Nefarius.DSharpPlus.Extensions.Hosting) ![Nuget](https://img.shields.io/nuget/dt/Nefarius.DSharpPlus.Extensions.Hosting) [Discord](https://img.shields.io/discord/346756263763378176.svg)](https://discord.vigem.org/)

## About

---

**This is not an official DSharpPlus extension, use it at your own risk!**

Work in progress! API-breaking changes might occur until the pre-release suffix disappears!

---

This set of libraries abstracts away a lot of the plumbing code required to get DSharpPlus up and running in a .NET Core Worker Service (or simply a plain old Console Application) and provides Dependency-Injection-friendly integration.

It also offers a new feature/concept on top: event subscribers! Changes happening in the Discord universe are represented in DSharpPlus by a (rather large) number of events that can be subscribed to. The library offers an interface for every event of the Discord Client your "event subscriber" class can implement. These interface methods will be called when the corresponding event occurs. But there's a convenience extra: within the callback method you will have access to scoped services, like database contexts!

And if that wasn't enough, here's another one: intents will be automatically registered if you're using an interface/event that requires them! Yay automation!

## To-Do

- [ ] Documentation
- [ ] Support the sharded client

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

Optional. Adds support for [DSharpPlus.VoiceNext](https://dsharpplus.github.io/articles/audio/voicenext/prerequisites.html) extension.

### Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting

[![NuGet](https://img.shields.io/nuget/vpre/Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting.svg)](https://nuget.org/packages/Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting)

Optional. Adds support for [DSharpPlus.SlashCommands](https://github.com/DSharpPlus/DSharpPlus/tree/master/DSharpPlus.SlashCommands) extension.

## Documentation

If you're already familiar with .NET Core Workers or ASP.NET Core you'll have your bot up and running in seconds 👌

You can also take a look at [the reference example](WorkerExample) of this repository.

### Setup

Create a new .NET Core Worker project either via Visual Studio templates or using the command `dotnet new worker` in a fresh directory.

The current version of the library depends on the DSharpPlus nightly version. If you're using the stable nuget version, [update to the nightly version](https://dsharpplus.github.io/articles/misc/nightly_builds.html).

Add the core hosting package (and optional extensions, if you need them) via NuGet package manager.

### Implementation

Most of the heavy lifting is done in the `ConfigureServices` method, so we will focus on that. To get a bare basic Discord bot running, all you need to do is register the client service and the hosted background service:

```csharp
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

### OpenTracing (Optional)

You probably wonder what's the deal with the tracing dependency. I've taken liberty to implement [OpenTracing](https://github.com/opentracing/opentracing-csharp) within all event subscribers, so if your bot struggles with performance, you can easily analyse it with the addition of e.g. [Jaeger Tracing](https://github.com/jaegertracing/jaeger-client-csharp). If you don't know what that means or don't care about tracing at all, just skip this section. To utilise tracing, you can use this snippet to get you up and running (although I highly recommend you check out the Jaeger docs beforehand anyway):

```csharp
// Adds the Jaeger Tracer.
services.AddSingleton<ITracer>(serviceProvider =>
{
 var serviceName = "MyBot";
 var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

 // This is necessary to pick the correct sender, otherwise a NoopSender is used!
 Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
  .RegisterSenderFactory<ThriftSenderFactory>();

 // This will log to a default localhost installation of Jaeger.
 var tracer = new Tracer.Builder(serviceName)
  .WithLoggerFactory(loggerFactory)
  .WithSampler(new ConstSampler(true))
  .Build();

 // Allows code that can't use DI to also access the tracer.
 GlobalTracer.Register(tracer);

 return tracer;
});
```

Make sure you call this **before** `AddDiscord` or the Mock Tracer will be used by default which will not collect or publish any metrics.

### Handling Discord Events

Now to the actual convenience feature of this library! Creating one (or more) class(es) that handle events, like when a guild came online or a message got created. Let's wire one up that gets general guild and member change events:

```csharp
// this does the same as calling
//   services.AddDiscordGuildAvailableEventSubscriber<BotModuleForGuildAndMemberEvents>();
[DiscordGuildAvailableEventSubscriber]
// this does the same as calling
//   services.AddDiscordGuildMemberAddedEventSubscriber<BotModuleForGuildAndMemberEvents>();
[DiscordGuildMemberAddedEventSubscriber]
internal class BotModuleForGuildAndMemberEvents :
    // you can implement one or many interfaces for event handlers in one class 
    // or split it however you like. Your choice!
    IDiscordGuildAvailableEventSubscriber,
    IDiscordGuildMemberAddedEventSubscriber
{
    private readonly ILogger<BotModuleForGuildAndMemberEvents> _logger;

    private readonly ITracer _tracer;

    /// <summary>
    ///     Optional constructor for Dependency Injection.
    ///     Parameters get populated automatically with your services.
    /// </summary>
    /// <param name="logger">The logger service instance.</param>
    /// <param name="tracer">The tracer service instance.</param>
    public BotModuleForGuildAndMemberEvents(
        ILogger<BotModuleForGuildAndMemberEvents> logger,
        ITracer tracer
    )
    {
        //
        // Do whatever you like with these. It's recommended to not do heavy tasks in 
        // constructors, just store your service references for later use!
        // 
        // You can inject scoped services like database contexts as well!
        // 
        _logger = logger;
        _tracer = tracer;
    }

    public Task DiscordOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs args)
    {
        //
        // To see some action, output the guild name
        // 
        Console.WriteLine(args.Guild.Name);

        //
        // Usage of injected logger service
        // 
        _logger.LogInformation("Guild {Guild} came online", args.Guild);

        //
        // Return successful execution
        // 
        return Task.CompletedTask;
    }

    public Task DiscordOnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs args)
    {
        //
        // Fired when a new member has joined, exciting!
        // 
        _logger.LogInformation("New member {Member} joined!", args.Member);

        //
        // Return successful execution
        // 
        return Task.CompletedTask;
    }
}
```

Now let's dissect what is happening here. The class gets decorated by the attributes `DiscordGuildAvailableEventSubscriber` and `DiscordGuildMemberAddedEventSubscriber` (hint: you can use only one attribute for the event group you're interested in, you can use many more on the same class, doesn't matter, your choice) which causes it to get **automatically registered as subscribers for these events**.

An *alternative* approach to registration is manually calling the extension methods, like

```csharp
services.AddDiscordGuildAvailableEventSubscriber<BotModuleForGuildAndMemberEvents>();
services.AddDiscordGuildMemberAddedEventSubscriber<BotModuleForGuildAndMemberEvents>();
```

from within `ConfigureServices`. Using the attributes instead ensures you don't forget to register your subscribers while coding vigorously!

Implementing the interfaces `IDiscordGuildAvailableEventSubscriber` and `IDiscordGuildMemberEventsSubscriber` ensures your subscriber class is actually callable by the Discord Client Service. You must complete every event callback you're not interested in with `return Task.CompletedTask;` as demonstrated or it will result in errors. In the example above we are only interested in `DiscordOnGuildAvailable` and print the guild name to the console. I'm sure you can think of more exciting tasks!

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

You can even inject **scoped services**, the subscriber objects get invoked in their own scope by default. This allows for easy access for e.g. database contexts within each subscriber. Neat!
