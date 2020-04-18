using RedditBot.Models.Domain;

namespace RedditBot.Models.Repositories {
    public interface IPrefixRepository {
        public bool CheckIfGuildHasPrefix(ulong guildID);
        public Prefix GetPrefixForGuild(ulong guildID);
        public void AddPrefix(Prefix prefix);
        public void ChangePrefix(ulong guildID, char prefix);
    }
}
