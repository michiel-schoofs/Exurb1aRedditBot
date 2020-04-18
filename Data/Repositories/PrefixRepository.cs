using Microsoft.EntityFrameworkCore;
using RedditBot.Exceptions;
using RedditBot.Models.Domain;
using System.Linq;
using RedditBot.Models.Repositories;

namespace RedditBot.Data.Repositories {
    public class PrefixRepository : IPrefixRepository {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<Prefix> _prefix;

        public PrefixRepository(ApplicationDBContext context) {
            _context = context;
            _prefix = context.Prefixes;
        }

        public void AddPrefix(Prefix prefix) {
            if (CheckIfGuildHasPrefix(prefix.GuildID))
                throw new PrefixException("This guild already has a prefix");

            _prefix.Add(prefix);
            _context.SaveChanges();
        }

        public void ChangePrefix(ulong guildID, char prefix) {
            if(!CheckIfGuildHasPrefix(guildID))
                throw new PrefixException("This guild doesn't have a prefix");

            Prefix pref = GetPrefixForGuild(guildID);
            pref.PrefixCommand = prefix;
            _context.SaveChanges();
        }

        public bool CheckIfGuildHasPrefix(ulong guildID) {
            return GetPrefixForGuild(guildID) != null;
        }

        public Prefix GetPrefixForGuild(ulong guildID) {
            return _prefix.FirstOrDefault(p => p.GuildID == guildID);
        }
    }
}
