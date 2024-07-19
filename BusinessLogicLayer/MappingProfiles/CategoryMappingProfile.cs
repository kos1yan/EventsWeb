using AutoMapper;
using DataAccessLayer.Entities;
using Shared.DataTransferObjects.Category;

namespace BusinessLogicLayer.MappingProfiles
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
