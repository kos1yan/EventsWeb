
namespace Events.Domain.RequestFeatures
{
    public class EventParameters : RequestParameters
    {
        public string? SearchByName { get; set; }
        public string? Date { get; set; }
        public string? Adress { get; set; }
        public int? Category { get; set; }
    }
}
