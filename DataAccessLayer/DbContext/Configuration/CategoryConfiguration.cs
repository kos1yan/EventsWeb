using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DbContext.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(30).HasColumnName("name");

            builder.HasData(
                new Category
                {
                    Id = 1,
                    Name = "Sport"
                },
                new Category
                {
                    Id = 2,
                    Name = "Family"
                },
                new Category
                {
                    Id = 3,
                    Name = "Education"
                },
                new Category
                {
                    Id = 4,
                    Name = "Excursions"
                }
                );
        }
    }
}
