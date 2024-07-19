
namespace DataAccessLayer.Repositories
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        IEventRepository Event { get; }
        IMemberRepository Member { get; }
        ICloudinaryRepository Cloudinary { get; }
        Task SaveAsync();
    }
}
