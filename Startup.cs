using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Reddit.Controllers.EventArgs;
using RedditBot.Models.Domain;
using RedditBot.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace RedditBot {
    public class Startup {
        private readonly Secrets _secrets;
        private readonly DiscordSocketClient _client;
        private readonly RedditService _redditService;
        private readonly ServiceProvider _serviceProvider;
        private readonly CommandService _commands;
        private readonly char prefix = '*';

        public Startup(ServiceProvider provider) {
            _serviceProvider = provider;
            _secrets = provider.GetService<Secrets>();
            _redditService = provider.GetService<RedditService>();
            _client = new DiscordSocketClient();
            _commands = new CommandService();
        }

        public async void Configure() {
            _client.LoggedIn += _client_LoggedIn;
            _redditService.PostsUpdated += _redditService_PostsUpdated;
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services:_serviceProvider);
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage arg) {
            var message = arg as SocketUserMessage;

            if (message == null)
                return;

            int argPos = 0;
            if (!(message.HasCharPrefix(prefix, ref argPos)) || message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _serviceProvider);

            if (result.Error.HasValue) {
                if (result.Error == CommandError.Exception) {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }

        //To-DO
        private PostsUpdateEventArgs _redditService_PostsUpdated(PostsUpdateEventArgs eve) {
            throw new NotImplementedException();
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
