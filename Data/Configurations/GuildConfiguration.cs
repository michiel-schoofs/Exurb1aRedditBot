using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedditBot.Models.Domain;

namespace RedditBot.Data.Configurations {
    public class GuildConfiguration : IEntityTypeConfiguration<Guild> {

        public void Configure(EntityTypeBuilder<Guild> builder) {
            builder.ToTable("Guild");

            builder.HasKey(g => g.GuildID);
            builder.Property(g => g.GuildID).IsRequired();

            builder.Property(g => g.Name).IsRequired();
        }
    }
}
