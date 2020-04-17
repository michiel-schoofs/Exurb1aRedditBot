using Microsoft.EntityFrameworkCore;
using RedditBot.Data.Configurations;
using RedditBot.Models.Domain;

namespace RedditBot.Data {
    public class ApplicationDBContext: DbContext{
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Channel> Channels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new GuildConfiguration())
                .ApplyConfiguration(new ChannelConfiguration());
        }
    }
}
