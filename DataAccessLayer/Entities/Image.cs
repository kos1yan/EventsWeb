
namespace DataAccessLayer.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
    }
}
