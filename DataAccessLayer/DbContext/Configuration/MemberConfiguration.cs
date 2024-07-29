using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Events.Domain.Entities;

namespace Events.Infrastructure.DbContext.Configuration
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("member");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(30).HasColumnName("name");
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(30).HasColumnName("surname");
            builder.Property(x => x.DateOfBirth).IsRequired().HasColumnName("date_of_birth");
            builder.Property(x => x.Email).IsRequired().HasColumnName("email");
            builder.Property(x => x.RegistrationDate).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("registration_date");
            builder.HasOne(x => x.Event).WithMany(x => x.Members).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.User).WithMany(x => x.Members).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
