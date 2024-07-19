using Shared.DataTransferObjects.Image;


namespace Shared.DataTransferObjects.Event
{
    public record EventForReviewDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public int FreePlaces { get; set; }
        public bool IsSubscribed { get; set; }
        public List<ImageDto> Images { get; set; }
    }
}
