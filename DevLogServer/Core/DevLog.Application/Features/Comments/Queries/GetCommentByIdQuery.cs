using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Comments.Result;

namespace DevLog.Application.Features.Comments.Queries;

// tek bir yorum getirmek için sorgu
public record GetCommentByIdQuery(Guid Id) : IRequest<BaseResult<GetCommentByIdQueryResult>>;
