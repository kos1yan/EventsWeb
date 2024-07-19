using Shared.DataTransferObjects.Category;
using Shared.DataTransferObjects.Image;

namespace Shared.DataTransferObjects.Event
{
    public record EventDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? Adress { get; init; }
        public string? Date { get; init; }
        public bool IsSubscribed { get; set; }
        public int MaxMemberCount { get; init; }
        public int FreePlaces { get; set; }
        public CategoryDto Category { get; set; }
        public List<ImageDto> Images { get; set; }
    }
}
