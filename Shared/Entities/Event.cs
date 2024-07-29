
namespace Events.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Adress { get; set; }
        public string? Date { get; set; }
        public int MaxMemberCount { get; set; }
        public int MemberCount { get; set; }
        public List<Member> Members { get; set; }
        public List<Image> Images { get; set; } = new();
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
