using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RedditBot.Exceptions;
using RedditBot.Models.Domain;
using RedditBot.Models.Repositories;
using System.Linq;

namespace RedditBot.Data.Repositories {
    public class GuildRepository : IGuildRepository {
        private readonly ApplicationDBContext _context;
        private DbSet<Guild> _guilds;

        public GuildRepository(ApplicationDBContext context) {
            _context = context;
            _guilds = context.Guilds;
        }

        public void AddGuild(Guild guild) {
            if (Exists(guild.GuildID))
                throw new GuildException("This guild is already present in the database");

            _guilds.Add(guild);
            _context.SaveChanges();
        }

        public bool Exists(ulong id) => GetGuildById(id) != null;

        public IEnumerable<Guild> AllGuilds => _guilds.ToList();

        public Guild GetGuildById(ulong id) => _guilds.FirstOrDefault(g => g.GuildID == id);

        public void RemoveGuild(Guild guild) {
            if (!Exists(guild.GuildID))
                throw new GuildException("The specified guild couldn't be deleted");

            _guilds.Remove(guild);
            _context.SaveChanges();
        }
    }
}
