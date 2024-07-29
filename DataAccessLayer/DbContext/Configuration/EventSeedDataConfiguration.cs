using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Events.Domain.Entities;

namespace Events.Infrastructure.DbContext.Configuration
{
    public class EventSeedDataConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {

            builder.HasData(
                new Event
                {
                    Id = new Guid("2430104a-8105-4ac0-9d05-853e1c3e1727"),
                    Name = "Tech Conference 2024",
                    Description = "Annual tech conference focusing on new advancements in AI and machine learning.",
                    Adress = "123 Tech Ave, San Francisco, CA",
                    Date = "2024-09-12T10:00",
                    MaxMemberCount = 50,
                    MemberCount = 0,
                    CategoryId = 1
                },
                new Event
                {
                    Id = new Guid("144ae60b-3b32-438d-a460-a719484bf379"),
                    Name = "Startup Pitch Night",
                    Description = "Local entrepreneurs pitch their startups to a panel of investors.",
                    Adress = "456 Innovation Blvd, New York, NY",
                    Date = "2024-09-12T10:00",
                    MaxMemberCount = 40,
                    MemberCount = 0,
                    CategoryId = 3
                },
                new Event
                {
                    Id = new Guid("7e1a535e-3fc2-4ef4-afe0-a14e2623e1cc"),
                    Name = "Healthcare Symposium",
                    Description = "A symposium discussing the latest in healthcare technology and innovations.",
                    Adress = "789 Health St, Boston, MA",
                    Date = "2024-09-14T11:00",
                    MaxMemberCount = 45,
                    MemberCount = 0,
                    CategoryId = 3
                },
                new Event
                {
                    Id = new Guid("fcca46fa-1b57-4365-a63a-da1d52c5c232"),
                    Name = "Art & Design Expo",
                    Description = "An expo showcasing the latest trends in art and design.",
                    Adress = "321 Creative Way, Los Angeles, CA",
                    Date = "2024-09-19T19:00",
                    MaxMemberCount = 34,
                    MemberCount = 0,
                    CategoryId = 4
                },
                new Event
                {
                    Id = new Guid("69798c4d-3f35-4f99-8b7f-9dd938a877d7"),
                    Name = "Blockchain Summit",
                    Description = "A summit focused on the future of blockchain technology.",
                    Adress = "654 Crypto Ln, Miami, FL",
                    Date = "2024-09-11T12:00",
                    MaxMemberCount = 46,
                    MemberCount = 0,
                    CategoryId = 3
                },
                new Event
                {
                    Id = new Guid("01d1e9bc-d270-4765-99e1-abad5e01a819"),
                    Name = "Sustainability Conference",
                    Description = "Conference on sustainable practices and green technology.",
                    Adress = "987 Green Rd, Seattle, WA",
                    Date = "2024-09-15T14:30",
                    MaxMemberCount = 30,
                    MemberCount = 0,
                    CategoryId = 3
                },
                new Event
                {
                    Id = new Guid("13fc7bd2-7860-470c-960b-0f775d41a375"),
                    Name = "Music Festival 2024",
                    Description = "An outdoor music festival featuring various artists and genres.",
                    Adress = "123 Festival Park, Austin, TX",
                    Date = "2024-09-13T11:00",
                    MaxMemberCount = 40,
                    MemberCount = 0,
                    CategoryId = 2
                },
                new Event
                {
                    Id = new Guid("58bc1330-b7f6-40a0-9278-15a396483a0d"),
                    Name = "Literature Fair",
                    Description = "A fair celebrating literature with author readings and book signings.",
                    Adress = "456 Book St, Chicago, IL",
                    Date = "2024-09-11T11:00",
                    MaxMemberCount = 40,
                    MemberCount = 0,
                    CategoryId = 2
                },
                new Event
                {
                    Id = new Guid("9361dafc-bc5d-49f1-850a-c789ce47ce0b"),
                    Name = "Film Festival",
                    Description = "A festival screening independent films and documentaries.",
                    Adress = "789 Cinema Blvd, Portland, OR",
                    Date = "2024-09-13T11:00",
                    MaxMemberCount = 45,
                    MemberCount = 0,
                    CategoryId = 2
                },
                new Event
                {
                    Id = new Guid("306b0d5d-1802-413e-ac2b-2935847b0736"),
                    Name = "Gaming Convention",
                    Description = "A convention for video game enthusiasts and developers.",
                    Adress = "321 Game St, Las Vegas, NV",
                    Date = "2024-09-13T17:00",
                    MaxMemberCount = 50,
                    MemberCount = 0,
                    CategoryId = 1
                });
        }
    }
}
