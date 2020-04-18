using RedditBot.Models.Domain;
using System.Collections.Generic;

namespace RedditBot.Models.Repositories {
    public interface IGuildRepository {
        IEnumerable<Guild> AllGuilds { get; }

        Guild GetGuildById(ulong id);
        
        void AddGuild(Guild guild);
        void RemoveGuild(Guild guild);

        bool Exists(ulong id);
    }
}
