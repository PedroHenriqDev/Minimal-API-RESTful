using AutoMapper;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Response;
using Catalogue.Domain.Entities;

namespace Catalogue.Application.Mappings.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<Category, CreateCategoryCommandRequest>().ReverseMap();
        CreateMap<Category, CreateCategoryCommandResponse>().ReverseMap();
        CreateMap<Category, DeleteCategoryCommandResponse>().ReverseMap();
    }
}
