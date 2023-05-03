using AutoMapper;
using Catalog.API.Dtos;
using Catalog.API.Models;

namespace Catalog.API.Mapping;

public class GeneralMapping:Profile
{
    public GeneralMapping()
    {
        CreateMap<Course,CourseDto>().ReverseMap(); //Course -> CourseDto ya çevrilir ya da tam tersi yapılabilir.
        CreateMap<CourseCreateDto,Course>(); //olmalı
        CreateMap<CourseUpdateDto,Course>();

        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CategoryCreateDto,Category>();
        CreateMap<Feature, FeatureDto>().ReverseMap();
    
    }
}
