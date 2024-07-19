using AutoMapper;
using DataAccessLayer.Entities;
using Shared.DataTransferObjects.Image;

namespace BusinessLogicLayer.MappingProfiles
{
    public class ImageMappingProfile : Profile
    {
        public ImageMappingProfile()
        {
            CreateMap<Image, ImageDto>();
        }
    }
}
