using RedditBot.Models.Domain;
using System.Collections.Generic;

namespace RedditBot.Models.Repositories {
    public interface IChannelRepository {
        void AddChannel(Channel channel);
        void RemoveChannel(ulong id);
        IEnumerable<Channel> GetAllChannels();
        IEnumerable<Channel> GetAllFromGuild(ulong guildId);
        Channel GetChannelByID(ulong id);
        IEnumerable<Channel> GetChannelByType(ChannelType type);
        Channel GetChannelByTypeInGuild(ulong guildId, ChannelType type);
        bool Exists(ulong id);
    }
}
