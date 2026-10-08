using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Blogs.Result;

namespace DevLog.Application.Features.Blogs.Queries;

// blog listesini getiren sorgu (sadece okuma yapıyor, CQRS'in Q kısmı)
public record GetBlogsByCategoryIdQuery(Guid CategoryId) : IRequest<BaseResult<List<GetBlogsByCategoryIdQueryResult>>>;
