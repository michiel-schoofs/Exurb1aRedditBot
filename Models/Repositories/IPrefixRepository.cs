using RedditBot.Models.Domain;

namespace RedditBot.Models.Repositories {
    public interface IPrefixRepository {
        bool CheckIfGuildHasPrefix(ulong guildID);
        Prefix GetPrefixForGuild(ulong guildID);
        void AddPrefix(Prefix prefix);
        void ChangePrefix(ulong guildID, char prefix);
    }
}
