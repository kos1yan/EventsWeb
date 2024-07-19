using AutoMapper;
using DataAccessLayer.Entities;
using Shared.DataTransferObjects.Member;

namespace BusinessLogicLayer.MappingProfiles
{
    public class MemberMappingProfile : Profile
    {
        public MemberMappingProfile()
        {
            CreateMap<MemberForCreationDto, Member>();
            CreateMap<Member, MemberDto>();
        }
    }
}
