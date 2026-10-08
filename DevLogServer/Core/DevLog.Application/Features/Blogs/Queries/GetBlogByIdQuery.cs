using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Blogs.Result;

namespace DevLog.Application.Features.Blogs.Queries;

// tek bir blog getirmek için sorgu
public record GetBlogByIdQuery(Guid Id) : IRequest<BaseResult<GetBlogByIdQueryResult>>;
