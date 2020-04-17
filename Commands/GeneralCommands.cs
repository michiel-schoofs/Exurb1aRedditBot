using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace RedditBot.Commands {
    public class GeneralCommands : ModuleBase<SocketCommandContext> {
        [Command("ping")]
        [Summary("command that's used to check if the bot is alive")]
        public async Task Ping([Remainder] string x="") {
            TimeSpan diff = DateTime.Now.ToUniversalTime().Subtract(Context.Message.Timestamp.UtcDateTime);
            await Context.Channel.SendMessageAsync(string.Format("pong 🏓 ({0} ms)",+ Math.Round(diff.TotalMilliseconds)));
        }        
        
        [Command("pong")]
        [Summary("People think they're original")]
        public async Task Pong([Remainder] string x="") {
            await Context.Channel.SendMessageAsync("ping 🏓");
        }
    }
}
