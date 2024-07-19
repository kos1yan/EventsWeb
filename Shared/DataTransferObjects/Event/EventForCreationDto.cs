using Microsoft.AspNetCore.Http;


namespace Shared.DataTransferObjects.Event
{
    public record EventForCreationDto
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? Adress { get; init; }
        public string? Date { get; init; }
        public int MaxMemberCount { get; init; }
        public int CategoryId { get; init; }
        public List<IFormFile>? Images { get; set; }
    }
}
