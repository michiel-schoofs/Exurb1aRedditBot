using Discord;
using Discord.WebSocket;
using Reddit.Controllers.EventArgs;
using RedditBot.Models.Domain;
using RedditBot.Services;
using System;
using System.Threading.Tasks;

namespace RedditBot {
    public class Startup {
        private readonly Secrets _secrets;
        private readonly DiscordSocketClient _client;
        private readonly RedditService _redditService;

        public Startup(Secrets secrets, RedditService redditService) {
            _secrets = secrets;
            _client =  new DiscordSocketClient();
            _redditService = redditService;
        }

        public void Configure() {
            _client.LoggedIn += _client_LoggedIn;
            _redditService.PostsUpdated += _redditService_PostsUpdated;      
        }

        //To-DO
        private PostsUpdateEventArgs _redditService_PostsUpdated(PostsUpdateEventArgs eve) {
            throw new NotImplementedException();
            IGuild
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
