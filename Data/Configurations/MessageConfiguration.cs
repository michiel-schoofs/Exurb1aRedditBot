using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedditBot.Models.Domain;

namespace RedditBot.Data.Configurations {
    public class MessageConfiguration : IEntityTypeConfiguration<Message> {
        public void Configure(EntityTypeBuilder<Message> builder) {
            builder.ToTable("Message");

            builder.HasKey(m => m.MessageID);
            builder.Property(m => m.MessageID).IsRequired();

            builder.HasOne(m => m.Channel)
                .WithMany()
                .HasPrincipalKey(c => c.ChanelID)
                .HasForeignKey(m => m.ChannelID)
                .HasConstraintName("ChannelID")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
