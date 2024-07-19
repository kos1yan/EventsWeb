
namespace Shared.DataTransferObjects.User
{
    public record RegistrationDto
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string? DeviceToken { get; init; }
    }
}
