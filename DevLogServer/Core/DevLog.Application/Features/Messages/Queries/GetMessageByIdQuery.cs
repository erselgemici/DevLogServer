using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Messages.Result;

namespace DevLog.Application.Features.Messages.Queries;

// tek bir mesaj getirmek için sorgu
public record GetMessageByIdQuery(Guid Id) : IRequest<BaseResult<GetMessageByIdQueryResult>>;
