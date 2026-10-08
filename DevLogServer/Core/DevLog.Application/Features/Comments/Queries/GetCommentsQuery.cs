using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Comments.Result;

namespace DevLog.Application.Features.Comments.Queries;

// yorum listesini getiren sorgu (sadece okuma yapıyor, CQRS'in Q kısmı)
public record GetCommentsQuery : IRequest<BaseResult<List<GetCommentsQueryResult>>>;
