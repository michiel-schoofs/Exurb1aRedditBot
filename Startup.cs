using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
using RedditBot.Data.Repositories;
using RedditBot.Models.Domain;
using RedditBot.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using RedditBot.Models.Repositories;
using Discord.Rest;

namespace RedditBot {
    public class Startup {
        private readonly Secrets _secrets;
        private readonly DiscordSocketClient _client;
        private readonly RedditService _redditService;
        private readonly ServiceProvider _serviceProvider;
        private readonly IGuildRepository _guildRepository;
        private readonly IChannelRepository _channelRepo;
        private readonly IMessageRepository _messageRepo;
        private readonly CommandService _commands;
        private List<Message> _messages;
        private readonly IEmote check = new Emoji("✅");
        private readonly IPrefixRepository _prefixRepository;
        private readonly IEmote cross = new Emoji("❌");
        private readonly char prefix = '*';
        private readonly string[] postmediaTypes = {"image","hosted:video", "rich:video" };

        public Startup(ServiceProvider provider) {
            _serviceProvider = provider;
            _secrets = provider.GetService<Secrets>();
            _redditService = provider.GetService<RedditService>();
            _channelRepo = provider.GetService<IChannelRepository>();
            _messageRepo = provider.GetService<IMessageRepository>();
            _prefixRepository = provider.GetService<IPrefixRepository>();
            _guildRepository = provider.GetService<IGuildRepository>();
            _client = new DiscordSocketClient();
            _commands = new CommandService();
        }

        public async void Configure() {
            _client.JoinedGuild += _client_JoinedGuild;
            _messages = _messageRepo.GetAllMessages();
            _client.LoggedIn += _client_LoggedIn;
            _redditService.PostsUpdated += _redditService_PostsUpdated;
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _serviceProvider);
            _client.MessageReceived += HandleCommandAsync;
            _client.ReactionAdded += _client_ReactionAdded;
        }

        //Set default prefix
        private async Task _client_JoinedGuild(SocketGuild arg) {
            if (!_guildRepository.Exists(arg.Id))
                _guildRepository.AddGuild(new Guild() { GuildID = arg.Id, Name = arg.Name });

            Guild guild = _guildRepository.GetGuildById(arg.Id);

            if (!_prefixRepository.CheckIfGuildHasPrefix(guild.GuildID)) {
                Prefix pref = new Prefix() {
                    PrefixCommand = prefix,
                    Guild = guild,
                    GuildID = guild.GuildID
                };

                _prefixRepository.AddPrefix(pref);
            } else {
                _prefixRepository.ChangePrefix(arg.Id, prefix);
            }

            return;
        }


        private async Task _client_ReactionAdded(
            Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3
            ) {
            var curratedReactions = new string[] { check.Name , cross.Name };

            if (!_messages.Select(m => m.MessageID).Contains(arg1.Id))
                return;

            if (arg3.User.Value.IsBot || !curratedReactions.Contains(arg3.Emote.Name))
                return;

            HandleMessage(arg1.DownloadAsync().Result, arg3.Emote);
        }

        private async Task HandleMessage(IUserMessage msg, IEmote reaction) {
            Message mes = _messageRepo.GetMessage(msg.Id);

            _messageRepo.RemoveMessage(mes.MessageID);
            _messages.Remove(_messages.FirstOrDefault(m=>m.MessageID == msg.Id));
            Embed embeds = (Embed)msg.Embeds.First();

            if (reaction.Name.Equals(cross.Name)) {
                var changed = embeds.ToEmbedBuilder();
                changed.Color = Color.Red;

                await msg.RemoveAllReactionsAsync();
                await msg.ModifyAsync(m => m.Embed = changed.Build());
                return; 
            }


            await msg.DeleteAsync();

            Channel chan = _channelRepo.GetChannelByTypeInGuild(mes.Channel.GuildID, Models.Domain.ChannelType.publicfeed);
            SocketGuild guild = _client.Guilds.FirstOrDefault(g => g.Id == chan.GuildID);
            IMessageChannel channel = (IMessageChannel) guild.GetChannel(chan.ChanelID);
            await channel.SendMessageAsync(embed: embeds);
        }

        private async Task HandleCommandAsync(SocketMessage arg) {
            var message = arg as SocketUserMessage;
            var guild = arg.Channel as IGuildChannel;

            if (message == null)
                return;

            int argPos = 0;
            char prefix = '*';

            if (_prefixRepository.CheckIfGuildHasPrefix(guild.GuildId))
                prefix = _prefixRepository.GetPrefixForGuild(guild.GuildId).PrefixCommand;

            if (!(message.HasCharPrefix(prefix, ref argPos)) || message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _serviceProvider);

            if (result.Error.HasValue) {
                if (result.Error == CommandError.Exception) {
                    EmbedBuilder errorEmbed = EmbedService.BuildErrorEmbed(result.ErrorReason, context.Guild);
                    await context.Channel.SendMessageAsync(embed: errorEmbed.Build());
                }
            }
        }

        //To-DO
        private PostsUpdateEventArgs _redditService_PostsUpdated(PostsUpdateEventArgs eve) {

            IEnumerable<Channel> channels = _channelRepo.GetChannelByType(Models.Domain.ChannelType.approval);
            foreach (Channel chan in channels) {
                SocketGuild guild = _client.Guilds.FirstOrDefault(g => g.Id == chan.Guild.GuildID);
                if (guild != null) {
                    foreach (Post pos in eve.Added) {
                        var pos2 = pos.Listing;
                        Embed emb;

                        if (postmediaTypes.Contains(pos2.PostHint))
                            emb = EmbedService.BuildMediaRedditPost(pos, guild).Build();
                        else
                            emb = EmbedService.BuildRedditPost(pos, guild).Build();

                        ISocketMessageChannel chanel = (ISocketMessageChannel)guild.GetChannel(chan.ChanelID);
                        RestUserMessage msg = chanel.SendMessageAsync(embed: emb).Result;
                        msg.AddReactionAsync(check);
                        msg.AddReactionAsync(cross);

                        Message mesg = new Message() {
                            Channel = chan,
                            ChannelID = chan.ChanelID,
                            MessageID = msg.Id
                        };

                        _messageRepo.AddMessage(mesg);
                        _messages.Add(mesg);
                    }
                }
            }

            return eve;
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
