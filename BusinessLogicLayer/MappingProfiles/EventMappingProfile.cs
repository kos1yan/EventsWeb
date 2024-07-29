using AutoMapper;
using Events.Application.DataTransferObjects.Event;
using Events.Domain.Entities;

namespace Events.Application.MappingProfiles
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
