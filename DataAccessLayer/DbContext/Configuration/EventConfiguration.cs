using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.DbContext.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("event");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(30).HasColumnName("name");
            builder.Property(x => x.Description).IsRequired().HasMaxLength(200).HasColumnName("description");
            builder.Property(x => x.Adress).IsRequired().HasMaxLength(50).HasColumnName("adress");
            builder.Property(x => x.Date).HasColumnName("date");
            builder.Property(x => x.MemberCount).IsRequired().HasDefaultValue(0).HasColumnName("member_count");
            builder.Property(x => x.MaxMemberCount).IsRequired().HasColumnName("max_member_count");
            builder.HasOne(x => x.Category).WithMany(x => x.Events).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
