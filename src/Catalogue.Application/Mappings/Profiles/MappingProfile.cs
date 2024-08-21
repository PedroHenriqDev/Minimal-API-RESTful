using AutoMapper;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Domain.Entities;

namespace Catalogue.Application.Mappings.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<Category, CreateCategoryCommandRequest>().ReverseMap();
        CreateMap<Category, CreateCategoryCommandResponse>().ReverseMap();
        CreateMap<Category, DeleteCategoryCommandResponse>().ReverseMap();
        CreateMap<Category, UpdateCategoryCommandRequest>().ReverseMap();
        CreateMap<Category, UpdateCategoryCommandResponse>().ReverseMap();
    }
}
