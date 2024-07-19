
namespace Shared.DataTransferObjects.User
{
    public record LogInDto
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
