using Discord;
using Discord.WebSocket;
using RedditBot.Models.Domain;
using System;
using System.Threading.Tasks;

namespace RedditBot {
    public class Startup {
        private readonly Secrets _secrets;
        private readonly DiscordSocketClient _client;

        public Startup(Secrets secrets) {
            _secrets = secrets;
            _client =  new DiscordSocketClient();
        }

        public void Configure() {
            _client.LoggedIn += _client_LoggedIn;
        }

        private async Task _client_LoggedIn() {
            Console.WriteLine("Logged in to application");
        }

        public async Task Run() {
            await _client.LoginAsync(TokenType.Bot, _secrets.DiscordSecretID, true);
            await _client.StartAsync();
            await Task.Delay(-1);
        }
    }
}
