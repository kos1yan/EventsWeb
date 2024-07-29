using AutoMapper;
using Events.Application.DataTransferObjects.Member;
using Events.Domain.Entities;

namespace Events.Application.MappingProfiles
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
