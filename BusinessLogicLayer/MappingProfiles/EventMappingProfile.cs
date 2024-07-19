using AutoMapper;
using DataAccessLayer.Entities;
using Shared.DataTransferObjects.Event;

namespace BusinessLogicLayer.MappingProfiles
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<EventForCreationDto, Event>().ForMember(s => s.Images, opt => opt.Ignore());
            CreateMap<Event, EventDto>().ForMember(s => s.Images, opt => opt.Ignore());
            CreateMap<Event, EventForReviewDto>().ForMember(s => s.Images, opt => opt.Ignore());
            CreateMap<EventForUpdateDto, Event>().ForMember(s => s.Images, opt => opt.Ignore());
        }
    }
}
