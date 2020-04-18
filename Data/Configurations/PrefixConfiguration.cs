using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedditBot.Models.Domain;

namespace RedditBot.Data.Configurations {
    public class PrefixConfiguration : IEntityTypeConfiguration<Prefix> {
        public void Configure(EntityTypeBuilder<Prefix> builder) {
            builder.ToTable("Prefix");

            builder.HasKey(p => p.ID);
            builder.Property(p => p.ID)
                .ValueGeneratedOnAdd().IsRequired();

            builder.Property(p => p.PrefixCommand).IsRequired();

            builder
                .HasOne(g => g.Guild)
                .WithOne()
                .HasPrincipalKey<Guild>(g => g.GuildID)
                .HasForeignKey<Prefix>(p=>p.GuildID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
