using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RedditBot.Exceptions;
using System.Linq;
using RedditBot.Models.Domain;
using RedditBot.Models.Repositories;

namespace RedditBot.Data.Repositories {
    public class ChannelRepository : IChannelRepository {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<Channel> _channels;

        public ChannelRepository(ApplicationDBContext context) {
            _context = context;
            _channels = context.Channels;
        }

        public void AddChannel(Channel channel) {
            Channel chan = GetChannelByID(channel.ChanelID);

            if (chan != null && chan.Type == channel.Type)
                throw new ChannelException("You've already configured this channel");

            chan = GetChannelByTypeInGuild(channel.GuildID, channel.Type);

            if (chan != null)
                throw new ChannelException("There's already a type of this channel configured please remove the other channel first");

            _channels.Add(channel);
            _context.SaveChanges();
        }

        public bool Exists(ulong id) => _channels.FirstOrDefault(c => c.ChanelID == id) != null;

        public IEnumerable<Channel> GetAllChannels() => _channels.ToList();

        public IEnumerable<Channel> GetAllFromGuild(ulong guildId) => _channels.ToList().Where(c => c.GuildID == guildId);

        public Channel GetChannelByID(ulong id) => _channels.FirstOrDefault(c => c.ChanelID == id);

        public IEnumerable<Channel> GetChannelByType(ChannelType type) => _channels
            .Include(c=>c.Guild).ToList().Where(c => c.Type == type);

        public Channel GetChannelByTypeInGuild(ulong guildId, ChannelType type) => _channels.FirstOrDefault(c => c.Type == type && c.GuildID == guildId);

        public void RemoveChannel(ulong id) {
            if (!Exists(id))
                throw new ChannelException("The specified channel doesn't exist");

            Channel chan = GetChannelByID(id);
            _channels.Remove(chan);
            _context.SaveChanges();
        }
    }
}
