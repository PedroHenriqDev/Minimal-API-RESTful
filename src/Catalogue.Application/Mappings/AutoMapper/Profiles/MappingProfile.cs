using AutoMapper;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.Mappings.AutoMapper.Converts;
using Catalogue.Application.Pagination;
using Catalogue.Application.Products.Commands.Requests;
using Catalogue.Application.Products.Commands.Responses;
using Catalogue.Application.Products.Queries.Responses;
using Catalogue.Domain.Entities;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Catalogue.Application.Mappings.AutoMapper.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CreateCategoryCommandRequest>().ReverseMap();
        CreateMap<Category, CreateCategoryCommandResponse>().ReverseMap();
        CreateMap<Category, DeleteCategoryCommandResponse>().ReverseMap();
        CreateMap<Category, UpdateCategoryCommandRequest>().ReverseMap();
        CreateMap<Category, UpdateCategoryCommandResponse>().ReverseMap();
        CreateMap<Category, GetCategoryQueryResponse>().ReverseMap();

        CreateMap<Category, GetCategoryWithProductsQueryResponse>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
            .ReverseMap();

        CreateMap(typeof(PagedList<>), typeof(PagedList<>)).ConvertUsing(typeof(PagedListConverter<,>));

        CreateMap<Product, CreateProductCommandRequest>()
            .ForMember(dest => dest.CategoryName, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Product, CreateProductCommandResponse>().ReverseMap();
        CreateMap<Product, GetProductQueryResponse>().ReverseMap();
    }
}
