using AutoMapper;
using Events.Application.DataTransferObjects.Category;
using Events.Domain.Entities;

namespace Events.Application.MappingProfiles
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
