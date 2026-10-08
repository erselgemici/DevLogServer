using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.ContactInfos.Result;

namespace DevLog.Application.Features.ContactInfos.Queries;

// iletişim bilgisi listesini getiren sorgu (sadece okuma yapıyor, CQRS'in Q kısmı)
public record GetContactInfosQuery : IRequest<BaseResult<List<GetContactInfosQueryResult>>>;
