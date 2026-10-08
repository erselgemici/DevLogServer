using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.ContactInfos.Result;

namespace DevLog.Application.Features.ContactInfos.Queries;

// tek bir iletişim bilgisi getirmek için sorgu
public record GetContactInfoByIdQuery(Guid Id) : IRequest<BaseResult<GetContactInfoByIdQueryResult>>;
