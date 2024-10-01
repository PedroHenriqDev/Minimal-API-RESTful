namespace Catalogue.Application.Abstractions.Requests;

public abstract class GetCategoryQueryRequestBase
{
    public Guid Id {get; set;}        

    public GetCategoryQueryRequestBase(Guid id)
        => Id = id;
}