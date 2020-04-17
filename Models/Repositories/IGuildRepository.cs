using RedditBot.Models.Domain;
using System.Collections.Generic;

namespace RedditBot.Models.Repositories {
    public interface IGuildRepository {
        public IEnumerable<Guild> GetAllGuilds();
        public Guild GetGuildById(ulong id);
        
        public void AddGuild(Guild guild);
        public void RemoveGuild(Guild guild);

        public bool Exists(ulong id);
    }
}
