using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedditBot.Models.Domain;

namespace RedditBot.Data.Configurations {
    public class ChannelConfiguration : IEntityTypeConfiguration<Channel> {

        public void Configure(EntityTypeBuilder<Channel> builder) {
            builder.ToTable("Channel");

            builder.HasKey(c => c.ChanelID);

            builder.Property(c => c.ChanelID).IsRequired();
            builder.Property(c => c.Name).IsRequired();

            builder.Property(c => c.Type).HasConversion<int>();

            builder
                .HasOne(c => c.Guild)
                .WithMany()
                .HasPrincipalKey(g => g.GuildID)
                .HasForeignKey(c => c.GuildID)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
