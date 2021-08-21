using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

namespace Nefarius.DSharpPlus.Extensions.Hosting.Events.CommandsNext
{
    public interface IDiscordCommandsNextEventsSubscriber
    {
        public Task CommandsOnCommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs args);

        public Task CommandsOnCommandErrored(CommandsNextExtension sender, CommandErrorEventArgs args);
    }
}
