using AutoMapper;
using DataAccessLayer.Entities;
using Shared.DataTransferObjects.User;

namespace BusinessLogicLayer.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegistrationDto, User>().ForMember(s => s.UserName, opt => opt.MapFrom(s => s.Email));
        }
    }
}
