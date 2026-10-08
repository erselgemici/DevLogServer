using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Categories.Result;

namespace DevLog.Application.Features.Categories.Queries
{
    // kategori listesini getiren sorgu (sadece okuma yapıyor, CQRS'in Q kısmı)
    public class GetCategoryQuery : IRequest<BaseResult<List<GetCategoryQueryResult>>>
    {
    }
}
