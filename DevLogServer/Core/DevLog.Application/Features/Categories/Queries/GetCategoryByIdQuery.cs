using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Categories.Result;

namespace DevLog.Application.Features.Categories.Queries
{
    // tek bir kategori getirmek için sorgu
    public record GetCategoryByIdQuery(Guid Id) : IRequest<BaseResult<GetCategoryByIdQueryResult>>;

}
