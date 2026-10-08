using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.SubComments.Result;

namespace DevLog.Application.Features.SubComments.Queries;

// tek bir alt yorum getirmek için sorgu
public record GetSubCommentByIdQuery(Guid Id) : IRequest<BaseResult<GetSubCommentByIdQueryResult>>;
