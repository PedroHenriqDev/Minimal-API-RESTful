using AutoMapper;
using Catalogue.Application.Pagination;

namespace Catalogue.Application.Mappings.AutoMapper.Converts;

public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
{
    private readonly IMapper _mapper;

    public PagedListConverter(IMapper mapper)
    {
        _mapper = mapper;
    }

    public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
    {
        var items = _mapper.Map<IEnumerable<TDestination>>(source);

        return PagedList<TDestination>.ToPagedList
        (
            source.PageCurrent,
            source.PageSize,
            items.AsQueryable()
        );
    }
}
