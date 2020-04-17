using RedditBot.Models.Domain;
using System.Collections.Generic;

namespace RedditBot.Models.Repositories {
    public interface IChannelRepository {
        public void AddChannel(Channel channel);
        public void RemoveChannel(ulong id);
        public IEnumerable<Channel> GetAllChannels();
        public IEnumerable<Channel> GetAllFromGuild(ulong guildId);
        public Channel GetChannelByID(ulong id);
        public IEnumerable<Channel> GetChannelByType(ChannelType type);
        public Channel GetChannelByTypeInGuild(ulong guildId, ChannelType type);
        public bool Exists(ulong id);
    }
}
