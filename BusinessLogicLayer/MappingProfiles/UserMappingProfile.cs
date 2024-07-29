using AutoMapper;
using Events.Application.DataTransferObjects.User;
using Events.Domain.Entities;

namespace Events.Application.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegistrationDto, User>().ForMember(s => s.UserName, opt => opt.MapFrom(s => s.Email));
        }
    }
}
