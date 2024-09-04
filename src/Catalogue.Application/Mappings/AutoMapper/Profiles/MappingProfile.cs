using AutoMapper;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.DTOs.Requests;
using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Mappings.AutoMapper.Converts;
using Catalogue.Application.Pagination;
using Catalogue.Application.Products.Commands.Requests;
using Catalogue.Application.Products.Commands.Responses;
using Catalogue.Application.Products.Queries.Responses;
using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Users.Commands.Responses;
using Catalogue.Application.Users.Queries.Responses;
using Catalogue.Application.Utils;
using Catalogue.Domain.Entities;

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
        CreateMap<Category, CategoryResponse>().ReverseMap();

        CreateMap<Category, CreateCategoryWithProdsCommandRequest>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
            .ReverseMap();

        CreateMap<Category, CreateCategoryWithProdsCommandResponse>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
            .ReverseMap();

        CreateMap<Category, GetCategoryWithProdsQueryResponse>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
            .ReverseMap();

        CreateMap(typeof(PagedList<>), typeof(PagedList<>)).ConvertUsing(typeof(PagedListConverter<,>));

        CreateMap<Product, CreateProductByCatNameCommandRequest>()
            .ForMember(dest => dest.CategoryName, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Product, ProductRequest>().ReverseMap();
        CreateMap<Product, ProductResponse>().ReverseMap();
        CreateMap<Product, CreateProductCommandRequest>().ReverseMap();
        CreateMap<Product, CreateProductCommandResponse>().ReverseMap();
        CreateMap<Product, GetProductQueryResponse>().ReverseMap();
        CreateMap<Product, UpdateProductCommandRequest>().ReverseMap();
        CreateMap<Product, UpdateProductCommandResponse>().ReverseMap();
        CreateMap<Product, DeleteProductCommandResponse>().ReverseMap();
        CreateMap<Product, GetProductWithCatQueryResponse>().ReverseMap();

        CreateMap<RegisterUserCommandRequest, User>()
             .AfterMap((src, dest) => dest.Password = Crypto.Encrypt(src.Password))
             .ReverseMap();

        CreateMap<User, UserResponse>().ReverseMap();
        CreateMap<User, RegisterUserCommandResponse>().ReverseMap();

        CreateMap<User, LoginQueryResponse>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));

        CreateMap<User, UpdateUserRoleCommandResponse>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
            .ReverseMap();    
    }
}
