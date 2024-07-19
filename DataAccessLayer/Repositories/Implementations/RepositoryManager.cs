using DataAccessLayer.DbContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Entities.ConfigurationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DataAccessLayer.Repositories.Implementations
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly EventContext _eventContext;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IEventRepository> _eventRepository;
        private readonly Lazy<IMemberRepository> _memberRepository;
        private readonly Lazy<ICloudinaryRepository> _cloudinaryRepository;

        public RepositoryManager(EventContext eventContext, UserManager<User> userManager, IOptions<CloudinarySettings> cloudinarySettings)
        {
            _eventContext = eventContext;
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(userManager));
            _eventRepository = new Lazy<IEventRepository>(() => new EventRepository(eventContext));
            _memberRepository = new Lazy<IMemberRepository>(() => new MemberRepository(eventContext));
            _cloudinaryRepository = new Lazy<ICloudinaryRepository>(() => new CloudinaryRepository(cloudinarySettings));
        }

        public IUserRepository User => _userRepository.Value;
        public IEventRepository Event => _eventRepository.Value;
        public IMemberRepository Member => _memberRepository.Value;
        public ICloudinaryRepository Cloudinary => _cloudinaryRepository.Value;

        public async Task SaveAsync() => await _eventContext.SaveChangesAsync();
    }
}
