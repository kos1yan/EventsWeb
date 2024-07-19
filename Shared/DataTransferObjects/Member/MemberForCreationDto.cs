
namespace Shared.DataTransferObjects.Member
{
    public record MemberForCreationDto
    {
        public string? Name { get; init; }
        public string? Surname { get; init; }
        public string? DateOfBirth { get; init; }
        public string? Email { get; init; }
    }
}
