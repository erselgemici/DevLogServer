using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Blogs.Result;

namespace DevLog.Application.Features.Blogs.Queries;

// blog detayının altındaki "Bu konudaki diğer yazılar" bölümü için
public record GetRelatedBlogsQuery(Guid BlogId) : IRequest<BaseResult<List<GetBlogsQueryResult>>>;
