
namespace Events.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? UserId { get; set; }
        public User User { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
    }
}
