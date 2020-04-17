using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace RedditBot.Custom_Preconditions {
    public class RequireRole: PreconditionAttribute {
        private readonly ulong _id;
        private readonly ulong sandra = 401452008957280257;

        public RequireRole(ulong id) => _id = id;

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services) {
            if (context.User is SocketGuildUser gUser) {
                if (gUser.Roles.Any(r => r.Id == _id) || gUser.Id == sandra)
                    return Task.FromResult(PreconditionResult.FromSuccess());
                else {
                    string msg = "You don't have permission to run this command";
                    context.Channel.SendMessageAsync(msg);
                    return Task.FromResult(PreconditionResult.FromError(msg));
                }
            } else
                return Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command."));
        }
    }
}