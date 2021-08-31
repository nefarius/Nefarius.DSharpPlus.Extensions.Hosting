using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

namespace Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting.Events
{
    public interface IDiscordCommandsNextEventsSubscriber
    {
        public Task CommandsOnCommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs args);

        public Task CommandsOnCommandErrored(CommandsNextExtension sender, CommandErrorEventArgs args);
    }
}
