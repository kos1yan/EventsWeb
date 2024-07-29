using AutoMapper;
using Events.Application.DataTransferObjects.Image;
using Events.Domain.Entities;

namespace Events.Application.MappingProfiles
{
    public class ImageMappingProfile : Profile
    {
        public ImageMappingProfile()
        {
            CreateMap<Image, ImageDto>();
        }
    }
}
