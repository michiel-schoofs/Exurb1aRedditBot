using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RedditBot.Custom_Preconditions;
using RedditBot.Models.Domain;
using RedditBot.Models.Repositories;
using RedditBot.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using ChannelType = RedditBot.Models.Domain.ChannelType;

namespace RedditBot.Commands {
    [RequireRole(700783756620988496)]
    [RequireRole(432183276493471744)]
    [RequireRole(668446475851661343)]
    [Group("config")]
    public class ConfigureCommands : ModuleBase<SocketCommandContext> {
        private readonly IChannelRepository _channelRepo;
        private readonly IGuildRepository _guildRepo;
        private readonly IPrefixRepository _prefixRepo;

        public ConfigureCommands(IChannelRepository channelRepo, IGuildRepository guildRepo,IPrefixRepository prefixRepository) {
            _channelRepo = channelRepo;
            _guildRepo = guildRepo;
            _prefixRepo = prefixRepository;
        }

        [Command("add channel")]
        public async Task AddChannelForApproval([Remainder] string type="") {
            string sanitized = type.ToLower().Trim();
            string[] enumNames = Enum.GetNames(typeof(ChannelType));

            if (!enumNames.Contains(sanitized))
                throw new Exception("This is an invalid type you can use the following channel types: "
                    + string.Join(" or ",enumNames));

            AddGuildIfNotExist(Context.Guild);

            ChannelType typeChan = (ChannelType) Enum.Parse(typeof(ChannelType), type.Trim().ToLower());
            Guild guild = _guildRepo.GetGuildById(Context.Guild.Id);

            Channel channel = new Channel() {
                ChanelID = Context.Channel.Id,
                Guild = guild,
                GuildID = guild.GuildID,
                Name = Context.Channel.Name,
                Type = typeChan
            };

            await Context.Channel.SendMessageAsync("Adding channel to db...");
            _channelRepo.AddChannel(channel);
            await Context.Channel.SendMessageAsync($"Channel configured as {Enum.GetName(typeof(ChannelType), typeChan)}");
        }

        [Command("remove channel")]
        public async Task RemoveChannel([Remainder] string msg = "") {
            AddGuildIfNotExist(Context.Guild);
            await Context.Channel.SendMessageAsync("Removing channel from db...");
            _channelRepo.RemoveChannel(Context.Channel.Id);
            await Context.Channel.SendMessageAsync($"Removed {Context.Channel.Name} from db");
        }

        private void AddGuildIfNotExist(SocketGuild guild) {
            if (!_guildRepo.Exists(guild.Id)) {
                Guild gui = new Guild() { GuildID = guild.Id , Name = guild.Name };
                _guildRepo.AddGuild(gui);
            }
        }

        [Command("prefix")]
        public Task ChangePrefix() {
            throw new Exception("You have to supply a prefix");
        }
        
        [Command("prefix")]
        public async Task ChangePrefix(string x) { 
            if(x.Length > 1)
                throw new Exception("You can only have one character as prefix");

            char pref = Convert.ToChar(x);
            
            Guild guild = _guildRepo.GetGuildById(Context.Guild.Id);
            _prefixRepo.ChangePrefix(guild.GuildID, pref);

            await Context.Channel.SendMessageAsync($"prefix changed to {pref}");
        }
    }
}
