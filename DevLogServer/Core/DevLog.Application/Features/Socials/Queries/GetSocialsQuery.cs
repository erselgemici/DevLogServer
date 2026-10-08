using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Socials.Result;

namespace DevLog.Application.Features.Socials.Queries;

// sosyal medya linki listesini getiren sorgu (sadece okuma yapıyor, CQRS'in Q kısmı)
public record GetSocialsQuery : IRequest<BaseResult<List<GetSocialsQueryResult>>>;
