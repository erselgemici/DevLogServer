using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.SubComments.Result;

namespace DevLog.Application.Features.SubComments.Queries;

// alt yorum listesini getiren sorgu (sadece okuma yapıyor, CQRS'in Q kısmı)
public record GetSubCommentsQuery : IRequest<BaseResult<List<GetSubCommentsQueryResult>>>;
