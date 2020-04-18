using Microsoft.EntityFrameworkCore;
using RedditBot.Data.Configurations;
using RedditBot.Models.Domain;

namespace RedditBot.Data {
    public class ApplicationDBContext : DbContext {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Prefix> Prefixes { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new GuildConfiguration())
                .ApplyConfiguration(new ChannelConfiguration())
                .ApplyConfiguration(new MessageConfiguration())
                .ApplyConfiguration(new PrefixConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
