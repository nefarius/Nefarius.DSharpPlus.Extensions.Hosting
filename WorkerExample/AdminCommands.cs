using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace WorkerExample;

[Group("admin")] // let's mark this class as a command group
[Description("Administrative commands.")] // give it a description for help purposes
[Hidden] // let's hide this from the eyes of curious users
[RequirePermissions(Permissions.ManageGuild)] // and restrict this to users who have appropriate permissions
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class AdminCommands : BaseCommandModule
{
    // all the commands will need to be executed as <prefix>admin <command> <arguments>

    // this command will be only executable by the bot's owner
    [Command("sudo")]
    [Description("Executes a command as another user.")]
    [Hidden]
    [RequireOwner]
    public async Task Sudo(CommandContext ctx, [Description("Member to execute as.")] DiscordMember member,
        [RemainingText] [Description("Command text to execute.")]
        string command)
    {
        // note the [RemainingText] attribute on the argument.
        // it will capture all the text passed to the command

        // let's trigger a typing indicator to let
        // users know we're working
        await ctx.TriggerTypingAsync();

        // get the command service, we need this for
        // sudo purposes
        CommandsNextExtension cmds = ctx.CommandsNext;

        // retrieve the command and its arguments from the given string
        Command cmd = cmds.FindCommand(command, out string customArgs);

        // create a fake CommandContext
        CommandContext fakeContext = cmds.CreateFakeContext(member, ctx.Channel, command, ctx.Prefix, cmd, customArgs);

        // and perform the sudo
        await cmds.ExecuteCommandAsync(fakeContext);
    }
}