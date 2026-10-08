using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Messages.Result;

namespace DevLog.Application.Features.Messages.Queries;

// mesaj listesini getiren sorgu (sadece okuma yapıyor, CQRS'in Q kısmı)
public record GetMessagesQuery : IRequest<BaseResult<List<GetMessagesQueryResult>>>;
